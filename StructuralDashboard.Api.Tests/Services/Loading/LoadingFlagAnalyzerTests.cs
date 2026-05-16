using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Api.Services.Loading;
using StructuralDashboard.Api.Tests.Fixtures;

namespace StructuralDashboard.Api.Tests.Services.Loading;

[TestFixture]
public class LoadingFlagAnalyzerTests
{
    private static LoadableBeam Loadable(string memberId, StructuralGraph graph)
    {
        var member = graph.Members[memberId];
        return new LoadableBeam
        {
            MemberId = memberId,
            StartNodeId = member.NodeIds[0],
            EndNodeId = member.NodeIds[^1],
            Span = 20
        };
    }

    [Test]
    public void NoTributaries_NoFlags()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var beam = Loadable("B1", graph);

        var flags = LoadingFlagAnalyzer.Analyze(beam, graph);
        Assert.That(flags, Is.Empty);
    }

    [Test]
    public void BothSidesZeroWidth_FlagsNoParallelFraming()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var beam = Loadable("B1", graph);
        beam.Tributaries.Add(NewTrib(0, "sl-1", 50, 50));
        beam.Tributaries.Add(NewTrib(0, "sl-1", 50, 50));

        var flags = LoadingFlagAnalyzer.Analyze(beam, graph);
        Assert.That(flags, Does.Contain(LoadingFlagAnalyzer.NoParallelFraming));
    }

    [Test]
    public void OneSideZero_FlagsOneSidedTributary()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var beam = Loadable("B1", graph);
        beam.Tributaries.Add(NewTrib(2.5, "sl-1", 50, 50));
        beam.Tributaries.Add(NewTrib(0, "sl-1", 50, 50));

        var flags = LoadingFlagAnalyzer.Analyze(beam, graph);
        Assert.That(flags, Does.Contain(LoadingFlagAnalyzer.OneSidedTributary));
    }

    [Test]
    public void AllTributariesMissingSurfaceLoad_FlagsNoFloorAbove()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var beam = Loadable("B1", graph);
        beam.Tributaries.Add(NewTrib(2.5, "", 0, 0));
        beam.Tributaries.Add(NewTrib(2.5, "", 0, 0));

        var flags = LoadingFlagAnalyzer.Analyze(beam, graph);
        Assert.That(flags, Does.Contain(LoadingFlagAnalyzer.NoFloorAbove));
    }

    [Test]
    public void MultipleSurfaceLoads_FlagsMultipleSurfaceLoads()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var beam = Loadable("B1", graph);
        beam.Tributaries.Add(NewTrib(2.5, "sl-1", 50, 50));
        beam.Tributaries.Add(NewTrib(2.5, "sl-2", 100, 100));

        var flags = LoadingFlagAnalyzer.Analyze(beam, graph);
        Assert.That(flags, Does.Contain(LoadingFlagAnalyzer.MultipleSurfaceLoads));
    }

    [Test]
    public void IncomingReactionAtInteriorNode_FlagsPointLoadAtNonEndStation()
    {
        // Joist on girder: girder gets a point load at its interior node.
        var model = ModelFixtures.TwoLevelJoistOnGirder();
        var graph = ModelFixtures.BuildGraph(model);
        var girder = Loadable("G", graph);

        // Manually add an incoming reaction at the joist landing point — the joist's end
        // is a node that's interior to the girder. Look it up.
        var girderMember = graph.Members["G"];
        var interiorNodeId = girderMember.NodeIds[1]; // [start, interior, end]
        girder.IncomingReactions.Add(new Reaction
        {
            FromMemberId = "J",
            AtNodeId = interiorNodeId,
            MagnitudeLbs = 2500,
            Direction = ReactionDirection.Down
        });

        var flags = LoadingFlagAnalyzer.Analyze(girder, graph);
        Assert.That(flags, Does.Contain(LoadingFlagAnalyzer.PointLoadAtNonEndStation));
    }

    [Test]
    public void IncomingReactionAtEndNode_DoesNotFlagNonEndStation()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var graph = ModelFixtures.BuildGraph(model);
        var beam = Loadable("B1", graph);
        beam.IncomingReactions.Add(new Reaction
        {
            FromMemberId = "UB",
            AtNodeId = beam.StartNodeId,
            MagnitudeLbs = 1000,
            Direction = ReactionDirection.Down
        });

        var flags = LoadingFlagAnalyzer.Analyze(beam, graph);
        Assert.That(flags, Does.Not.Contain(LoadingFlagAnalyzer.PointLoadAtNonEndStation));
    }

    private static LinearTributary NewTrib(double width, string sl, double dl, double ll) => new()
    {
        Width = width,
        SurfaceLoadId = sl,
        DeadLoadPsf = dl,
        LiveLoadPsf = ll
    };
}
