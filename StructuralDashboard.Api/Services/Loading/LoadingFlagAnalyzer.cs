using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Domain.Loading;

namespace StructuralDashboard.Api.Services.Loading;

/// <summary>
/// Inspects a LoadableBeam after tributary and accumulation passes and emits
/// diagnostic flag strings that describe conditions worth surfacing to a user.
/// Stateless / pure; held outside the LoadableBeam type so the domain stays clean.
/// </summary>
public static class LoadingFlagAnalyzer
{
    public const string NoParallelFraming = "NoParallelFraming";
    public const string OneSidedTributary = "OneSidedTributary";
    public const string NoFloorAbove = "NoFloorAbove";
    public const string MultipleSurfaceLoads = "MultipleSurfaceLoads";
    public const string PointLoadAtNonEndStation = "PointLoadAtNonEndStation";

    public static IReadOnlyList<string> Analyze(LoadableBeam beam, StructuralGraph graph)
    {
        var flags = new List<string>();

        int sidesWithWidth = beam.Tributaries.Count(t => t.Width > 0);
        int sidesWithoutWidth = beam.Tributaries.Count(t => t.Width <= 0);
        if (beam.Tributaries.Count >= 2 && sidesWithWidth == 0)
            flags.Add(NoParallelFraming);
        else if (sidesWithWidth >= 1 && sidesWithoutWidth >= 1)
            flags.Add(OneSidedTributary);

        if (beam.Tributaries.Count > 0 && beam.Tributaries.All(t => string.IsNullOrEmpty(t.SurfaceLoadId)))
            flags.Add(NoFloorAbove);

        var distinctSurfaceLoads = beam.Tributaries
            .Where(t => !string.IsNullOrEmpty(t.SurfaceLoadId))
            .Select(t => t.SurfaceLoadId)
            .Distinct()
            .Count();
        if (distinctSurfaceLoads > 1)
            flags.Add(MultipleSurfaceLoads);

        if (HasIncomingReactionAtNonEndStation(beam, graph))
            flags.Add(PointLoadAtNonEndStation);

        return flags;
    }

    private static bool HasIncomingReactionAtNonEndStation(LoadableBeam beam, StructuralGraph graph)
    {
        if (beam.IncomingReactions.Count == 0) return false;
        if (!graph.Members.TryGetValue(beam.MemberId, out var member)) return false;
        if (member.NodeIds.Count < 2) return false;
        var startId = member.NodeIds[0];
        var endId = member.NodeIds[^1];
        foreach (var r in beam.IncomingReactions)
        {
            if (r.AtNodeId != startId && r.AtNodeId != endId)
                return true;
        }
        return false;
    }
}
