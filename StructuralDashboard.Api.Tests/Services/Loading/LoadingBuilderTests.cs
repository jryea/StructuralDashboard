using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Services.Loading;
using StructuralDashboard.Api.Tests.Fixtures;
using StructuralDashboard.Shared.Contracts;
using StructuralDashboard.Shared.Shared;

namespace StructuralDashboard.Api.Tests.Services.Loading;

[TestFixture]
public class LoadingBuilderTests
{
    [Test]
    public void Build_OneBeamModel_ReturnsOneLoadableBeamWithNodeIdsAndSpan()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var sut = new LoadingBuilder();

        var beams = sut.Build(model, graph);

        Assert.That(beams, Has.Count.EqualTo(1));
        var b = beams[0];
        Assert.That(b.MemberId, Is.EqualTo("B1"));
        Assert.That(b.StartNodeId, Is.Not.Null.And.Not.Empty);
        Assert.That(b.EndNodeId, Is.Not.Null.And.Not.Empty);
        Assert.That(b.StartNodeId, Is.Not.EqualTo(b.EndNodeId));
        // 240 inches / 12 = 20 ft
        Assert.That(b.Span, Is.EqualTo(20.0).Within(1e-6));
        Assert.That(b.Tributaries, Is.Empty);
        Assert.That(b.IncomingReactions, Is.Empty);
        Assert.That(b.OutgoingReactions, Is.Empty);
    }

    [Test]
    public void Build_ResolvedNodeIdsMatchGraphMemberEndpoints()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var sut = new LoadingBuilder();

        var beams = sut.Build(model, graph);
        var b = beams.Single(x => x.MemberId == "B1");

        var gm = graph.Members["B1"];
        Assert.That(b.StartNodeId, Is.EqualTo(gm.NodeIds[0]));
        Assert.That(b.EndNodeId, Is.EqualTo(gm.NodeIds[^1]));
    }

    [Test]
    public void Build_BeamNotInGraph_FallsBackToCoordinateMatch()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        // Strip B1 out of graph.Members so the lookup falls back to coordinate matching.
        graph.Members.Remove("B1");

        var sut = new LoadingBuilder();
        var beams = sut.Build(model, graph);

        Assert.That(beams, Has.Count.EqualTo(1));
        var b = beams[0];
        Assert.That(b.MemberId, Is.EqualTo("B1"));
        Assert.That(b.StartNodeId, Is.Not.Null.And.Not.Empty);
        Assert.That(b.EndNodeId, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void Build_BeamWithoutMatchingGraphNode_IsSkipped()
    {
        var model = ModelFixtures.WallSupportedBeam();
        // Add a beam that nothing else touches; remove its entry from the graph and
        // place it where there are no other nodes to coordinate-match.
        var orphan = new Beam
        {
            Id = "B-orphan",
            StartPoint = new Point { X = 9000, Y = 9000, Z = 120 },
            EndPoint = new Point { X = 9240, Y = 9000, Z = 120 },
            LevelId = ModelFixtures.LevelL2
        };
        model.Elements.Beams.Add(orphan);

        // Use an empty graph (no nodes for the orphan) for this scenario.
        var graph = new StructuralGraph { ModelId = model.Id };
        var sut = new LoadingBuilder();

        var beams = sut.Build(model, graph);
        Assert.That(beams.Any(b => b.MemberId == "B-orphan"), Is.False);
    }

    [Test]
    public void Build_ZeroLengthBeam_IsSkipped()
    {
        var model = ModelFixtures.IsolatedBeam();
        model.Elements.Beams.Clear();
        model.Elements.Beams.Add(new Beam
        {
            Id = "B-zero",
            StartPoint = new Point { X = 0, Y = 0, Z = 120 },
            EndPoint = new Point { X = 0, Y = 0, Z = 120 },
            LevelId = ModelFixtures.LevelL2
        });

        var graph = ModelFixtures.BuildGraph(model);
        var sut = new LoadingBuilder();

        var beams = sut.Build(model, graph);
        Assert.That(beams, Is.Empty);
    }

    [Test]
    public void Build_BeamMissingEndpoint_IsSkipped()
    {
        // Build graph FIRST (so it doesn't choke on the null endpoint), then add the
        // malformed beam to the model. LoadingBuilder must skip it gracefully.
        var model = ModelFixtures.IsolatedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        model.Elements.Beams.Add(new Beam
        {
            Id = "B-null",
            StartPoint = new Point { X = 0, Y = 0, Z = 120 },
            EndPoint = null!,
            LevelId = ModelFixtures.LevelL2
        });

        var sut = new LoadingBuilder();
        var beams = sut.Build(model, graph);
        Assert.That(beams.Any(b => b.MemberId == "B-null"), Is.False);
    }

    [Test]
    public void Build_EmptyModel_ReturnsEmptyList()
    {
        var model = new StructuralModel { Id = "empty" };
        var graph = new StructuralGraph { ModelId = "empty" };
        var sut = new LoadingBuilder();

        var beams = sut.Build(model, graph);
        Assert.That(beams, Is.Empty);
    }
}
