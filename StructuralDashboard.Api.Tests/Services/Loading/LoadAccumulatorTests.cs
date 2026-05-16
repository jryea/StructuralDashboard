using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Api.Services.Loading;
using StructuralDashboard.Api.Tests.Fixtures;

namespace StructuralDashboard.Api.Tests.Services.Loading;

[TestFixture]
public class LoadAccumulatorTests
{
    private static LoadableBeam MakeLoadable(string memberId, StructuralGraph graph, double plf = 0)
    {
        var member = graph.Members[memberId];
        var start = graph.Nodes[member.NodeIds[0]];
        var end = graph.Nodes[member.NodeIds[^1]];
        double dx = end.X - start.X;
        double dy = end.Y - start.Y;
        double dz = end.Z - start.Z;
        double spanFt = Math.Sqrt(dx * dx + dy * dy + dz * dz) / 12.0;

        var beam = new LoadableBeam
        {
            MemberId = memberId,
            StartNodeId = member.NodeIds[0],
            EndNodeId = member.NodeIds[^1],
            Span = spanFt
        };
        if (plf > 0)
            beam.Tributaries.Add(new LinearTributary
            {
                Width = 1.0,
                SurfaceLoadId = "x",
                DeadLoadPsf = plf,   // 1 ft tributary × plf psf → plf
                LiveLoadPsf = 0
            });
        return beam;
    }

    [Test]
    public void SimpleBeam_NoIncomingLoads_HalfDistributedLoadAtEachEnd()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var beam = MakeLoadable("B1", graph, plf: 100); // 100 plf × 20 ft span = 2000 lbs total
        var sut = new LoadAccumulator();

        var byNode = sut.Run(new[] { beam }, graph);

        Assert.That(beam.IncomingReactions, Is.Empty);
        Assert.That(beam.OutgoingReactions, Has.Count.EqualTo(2));
        Assert.That(beam.OutgoingReactions.All(r => Math.Abs(r.MagnitudeLbs - 1000) < 1e-6), Is.True);
        Assert.That(byNode.ContainsKey(beam.StartNodeId), Is.True);
        Assert.That(byNode.ContainsKey(beam.EndNodeId), Is.True);
    }

    [Test]
    public void JoistOnGirder_GirderPicksUpJoistReactionAtMidSpan()
    {
        var model = ModelFixtures.TwoLevelJoistOnGirder();
        var graph = ModelFixtures.BuildGraph(model);
        // Joist J: 1000 plf × 5 ft (60") = 5000 lbs total → each end 2500 lbs
        var joist = MakeLoadable("J", graph, plf: 1000);
        // Girder G has no tributary itself (just transfers the joist's reaction)
        var girder = MakeLoadable("G", graph, plf: 0);
        var sut = new LoadAccumulator();

        sut.Run(new[] { joist, girder }, graph);

        // Joist outgoing total: 5000 lbs
        Assert.That(joist.OutgoingReactions.Sum(r => r.MagnitudeLbs), Is.EqualTo(5000).Within(1e-3));

        // Girder's incoming should contain the joist's reaction landing at mid-span
        Assert.That(girder.IncomingReactions, Is.Not.Empty);
        Assert.That(girder.IncomingReactions.Any(r => r.FromMemberId == "J"), Is.True);

        // The joist deposits 2500 lbs at the girder's mid-span (x=10 ft of a 20 ft girder).
        // R_start = R_end = 2500 × 0.5 = 1250 lbs each.
        Assert.That(girder.OutgoingReactions, Has.Count.EqualTo(2));
        foreach (var r in girder.OutgoingReactions)
            Assert.That(r.MagnitudeLbs, Is.EqualTo(1250).Within(1e-3));
    }

    [Test]
    public void TwoLevelStack_LowerBeamReceivesUpperBeamReactionsThroughColumns()
    {
        var model = ModelFixtures.TwoLevelColumnStack();
        var graph = ModelFixtures.BuildGraph(model);
        var upper = MakeLoadable("BU", graph, plf: 200);  // 200 × 20 = 4000 lbs → 2000 each end
        var lower = MakeLoadable("BL", graph, plf: 0);
        var sut = new LoadAccumulator();

        var byNode = sut.Run(new[] { upper, lower }, graph);

        // The lower beam picks up the upper beam's reactions, transferred through the columns.
        Assert.That(lower.IncomingReactions, Is.Not.Empty);
        Assert.That(lower.IncomingReactions.All(r => r.FromMemberId == "BU"), Is.True);
        // Upper deposited 2000 at each endpoint; columns transferred them down to BL's endpoints.
        // Both arrive at the lower beam's ENDS (zero moment arm) so they pass through 1:1.
        // R_start = R_end = 2000 lbs (no distributed load on BL).
        foreach (var r in lower.OutgoingReactions)
            Assert.That(r.MagnitudeLbs, Is.EqualTo(2000).Within(1e-3));
    }

    [Test]
    public void NoTributary_ProducesZeroReactions()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var beam = MakeLoadable("B1", graph, plf: 0);
        var sut = new LoadAccumulator();

        sut.Run(new[] { beam }, graph);

        Assert.That(beam.IncomingReactions, Is.Empty);
        Assert.That(beam.OutgoingReactions, Has.Count.EqualTo(2));
        Assert.That(beam.OutgoingReactions.All(r => r.MagnitudeLbs == 0), Is.True);
    }

    [Test]
    public void PointLoadAtEndNode_HasZeroMomentArm_AddsDirectlyToThatEnd()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var beam = MakeLoadable("B1", graph, plf: 0);

        // Pre-seed a reaction at the beam's start node (simulates a load from above
        // arriving at an end). This is the special-case "point load at zero moment arm".
        var pre = new Dictionary<string, IReadOnlyList<Reaction>>();
        // We can't pass a pre-seeded dict directly — fold the same scenario into a second beam.
        // Build a synthetic upstream beam at a higher elevation whose reaction lands at B1's start.

        // Simpler: use TwoLevelColumnStack and verify with zero-load upper beam + non-zero distributed on lower
        var stackModel = ModelFixtures.TwoLevelColumnStack();
        var stackGraph = ModelFixtures.BuildGraph(stackModel);
        var upper = MakeLoadable("BU", stackGraph, plf: 100); // 100 × 20 = 2000 lbs → 1000 each end
        var lower = MakeLoadable("BL", stackGraph, plf: 100); // same distributed load
        var sut = new LoadAccumulator();

        sut.Run(new[] { upper, lower }, stackGraph);

        // Upper deposits 1000 lbs at each end of lower (via columns). Those are at lower's ENDS.
        // Lower distributed = 100 × 20 = 2000 → 1000 lbs each end from its own distributed load.
        // Plus 1000 lbs at each end from upper.
        // Total per end = 1000 + 1000 = 2000 lbs.
        Assert.That(lower.OutgoingReactions, Has.Count.EqualTo(2));
        foreach (var r in lower.OutgoingReactions)
            Assert.That(r.MagnitudeLbs, Is.EqualTo(2000).Within(1e-3));
    }

    [Test]
    public void EmptyBeamList_ReturnsEmptyDictionary()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var sut = new LoadAccumulator();

        var byNode = sut.Run(Array.Empty<LoadableBeam>(), graph);
        Assert.That(byNode, Is.Empty);
    }

    [Test]
    public void JoistOnGirder_TopoSort_ProcessesJoistBeforeGirder()
    {
        // Pass beams in REVERSE order — accumulator must still process joist first.
        var model = ModelFixtures.TwoLevelJoistOnGirder();
        var graph = ModelFixtures.BuildGraph(model);
        var joist = MakeLoadable("J", graph, plf: 1000);
        var girder = MakeLoadable("G", graph, plf: 0);
        var sut = new LoadAccumulator();

        sut.Run(new[] { girder, joist }, graph);

        // If girder had been processed first, it would not have seen the joist reaction.
        // Verify the joist's reaction flowed into the girder's incoming list.
        Assert.That(girder.IncomingReactions.Any(r => r.FromMemberId == "J"), Is.True);
    }
}
