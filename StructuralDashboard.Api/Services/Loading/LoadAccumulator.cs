using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Domain.Loading;

namespace StructuralDashboard.Api.Services.Loading;

public sealed class LoadAccumulator : ILoadAccumulator
{
    private const double InchesPerFoot = 12.0;
    private const double ElevationEpsilon = 1e-6;

    private readonly ILogger<LoadAccumulator> _log;
    public LoadAccumulator(ILogger<LoadAccumulator>? logger = null) =>
        _log = logger ?? NullLogger<LoadAccumulator>.Instance;

    public IReadOnlyDictionary<string, IReadOnlyList<Reaction>> Run(
        IReadOnlyList<LoadableBeam> beams,
        StructuralGraph graph)
    {
        var reactionsByNode = new Dictionary<string, List<Reaction>>();
        if (beams is null || beams.Count == 0 || graph?.Members is null)
            return EmptyResult();

        var beamsByElevation = GroupBeamsByElevationDescending(beams, graph);
        _log.LogDebug("LoadAccumulator processing {BeamCount} beams across {LevelCount} elevations",
            beams.Count, beamsByElevation.Count);

        foreach (var (elevation, levelBeams) in beamsByElevation)
        {
            var ordered = TopoSortAtLevel(levelBeams, graph);
            foreach (var beam in ordered)
                ProcessBeam(beam, graph, reactionsByNode);

            PropagateThroughVerticalMembers(graph, reactionsByNode, fromElevation: elevation);
        }

        return reactionsByNode.ToDictionary(
            kv => kv.Key,
            kv => (IReadOnlyList<Reaction>)kv.Value.AsReadOnly());
    }

    private static IReadOnlyDictionary<string, IReadOnlyList<Reaction>> EmptyResult() =>
        new Dictionary<string, IReadOnlyList<Reaction>>();

    private static List<(double Elevation, List<LoadableBeam> Beams)> GroupBeamsByElevationDescending(
        IReadOnlyList<LoadableBeam> beams, StructuralGraph graph)
    {
        var groups = new Dictionary<double, List<LoadableBeam>>();
        foreach (var beam in beams)
        {
            if (!graph.Nodes.TryGetValue(beam.StartNodeId, out var startNode)) continue;
            var elev = startNode.NominalElevation;
            if (!groups.TryGetValue(elev, out var list))
                groups[elev] = list = new List<LoadableBeam>();
            list.Add(beam);
        }
        return groups
            .OrderByDescending(kv => kv.Key)
            .Select(kv => (kv.Key, kv.Value))
            .ToList();
    }

    /// <summary>
    /// Kahn's topological sort at a single elevation. A beam B' is upstream of B
    /// when B' has an endpoint at one of B's interior nodes (joist-on-girder).
    /// Same-elevation upstream beams must be processed first.
    /// </summary>
    private static List<LoadableBeam> TopoSortAtLevel(List<LoadableBeam> levelBeams, StructuralGraph graph)
    {
        var byId = levelBeams.ToDictionary(b => b.MemberId);
        var indegree = byId.Keys.ToDictionary(id => id, _ => 0);
        var downstreamOf = new Dictionary<string, List<string>>();

        foreach (var beam in levelBeams)
        {
            if (!graph.Members.TryGetValue(beam.MemberId, out var member)) continue;
            if (member.NodeIds.Count < 3) continue;

            for (int i = 1; i < member.NodeIds.Count - 1; i++)
            {
                var interiorNodeId = member.NodeIds[i];
                if (!graph.Nodes.TryGetValue(interiorNodeId, out var node)) continue;

                foreach (var otherMemberId in node.ConnectedMemberIds)
                {
                    if (otherMemberId == beam.MemberId) continue;
                    if (!byId.ContainsKey(otherMemberId)) continue;
                    if (!graph.Members.TryGetValue(otherMemberId, out var otherMember)) continue;
                    if (otherMember.NodeIds.Count < 2) continue;

                    bool otherEndsHere =
                        otherMember.NodeIds[0] == interiorNodeId ||
                        otherMember.NodeIds[^1] == interiorNodeId;
                    if (!otherEndsHere) continue;

                    indegree[beam.MemberId]++;
                    if (!downstreamOf.TryGetValue(otherMemberId, out var ds))
                        downstreamOf[otherMemberId] = ds = new List<string>();
                    ds.Add(beam.MemberId);
                }
            }
        }

        var queue = new Queue<string>(indegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
        var result = new List<LoadableBeam>();
        while (queue.Count > 0)
        {
            var id = queue.Dequeue();
            result.Add(byId[id]);
            if (!downstreamOf.TryGetValue(id, out var ds)) continue;
            foreach (var down in ds)
                if (--indegree[down] == 0) queue.Enqueue(down);
        }
        return result.Count == levelBeams.Count ? result : levelBeams.ToList();
    }

    private static void ProcessBeam(
        LoadableBeam beam,
        StructuralGraph graph,
        Dictionary<string, List<Reaction>> reactionsByNode)
    {
        if (!graph.Members.TryGetValue(beam.MemberId, out var member)) return;
        if (member.NodeIds.Count < 2) return;

        var startNodeId = member.NodeIds[0];
        var endNodeId = member.NodeIds[^1];
        if (!graph.Nodes.TryGetValue(startNodeId, out var startNode)) return;
        if (!graph.Nodes.TryGetValue(endNodeId, out _)) return;

        double L = beam.Span;
        if (L <= 0) return;

        double distLoadTotalLbs = beam.DistributedLoad * L;
        double rStart = distLoadTotalLbs / 2.0;
        double rEnd = distLoadTotalLbs / 2.0;

        foreach (var nodeId in member.NodeIds.ToList())
        {
            if (!reactionsByNode.TryGetValue(nodeId, out var rxns) || rxns.Count == 0) continue;
            if (!graph.Nodes.TryGetValue(nodeId, out var node)) continue;

            double xFeet = Distance(node, startNode) / InchesPerFoot;
            // Clamp to span to guard against tolerance drift on endpoint hits.
            if (xFeet < 0) xFeet = 0;
            if (xFeet > L) xFeet = L;

            foreach (var rxn in rxns)
            {
                beam.IncomingReactions.Add(rxn);
                double P = rxn.MagnitudeLbs;
                rEnd += P * xFeet / L;
                rStart += P * (L - xFeet) / L;
            }
            reactionsByNode.Remove(nodeId);
        }

        var outStart = new Reaction
        {
            FromMemberId = beam.MemberId,
            AtNodeId = startNodeId,
            MagnitudeLbs = rStart,
            Direction = ReactionDirection.Down
        };
        var outEnd = new Reaction
        {
            FromMemberId = beam.MemberId,
            AtNodeId = endNodeId,
            MagnitudeLbs = rEnd,
            Direction = ReactionDirection.Down
        };
        beam.OutgoingReactions.Add(outStart);
        beam.OutgoingReactions.Add(outEnd);

        AddReaction(reactionsByNode, startNodeId, outStart);
        AddReaction(reactionsByNode, endNodeId, outEnd);
    }

    /// <summary>
    /// Walks every graph member; for any segment with two endpoints at different
    /// elevations, copies reactions from the upper node down to the lower node
    /// when the upper node sits at <paramref name="fromElevation"/>. Multi-segment
    /// columns propagate one segment at a time as elevations get processed.
    /// </summary>
    private static void PropagateThroughVerticalMembers(
        StructuralGraph graph,
        Dictionary<string, List<Reaction>> reactionsByNode,
        double fromElevation)
    {
        foreach (var member in graph.Members.Values)
        {
            if (member.NodeIds.Count < 2) continue;

            for (int i = 0; i < member.NodeIds.Count - 1; i++)
            {
                var aId = member.NodeIds[i];
                var bId = member.NodeIds[i + 1];
                if (!graph.Nodes.TryGetValue(aId, out var nodeA)) continue;
                if (!graph.Nodes.TryGetValue(bId, out var nodeB)) continue;

                double aElev = nodeA.NominalElevation;
                double bElev = nodeB.NominalElevation;
                if (Math.Abs(aElev - bElev) < ElevationEpsilon) continue;

                var (topId, bottomId) = aElev > bElev ? (aId, bId) : (bId, aId);
                if (Math.Abs(graph.Nodes[topId].NominalElevation - fromElevation) > ElevationEpsilon) continue;

                if (!reactionsByNode.TryGetValue(topId, out var topRxns) || topRxns.Count == 0) continue;

                if (!reactionsByNode.TryGetValue(bottomId, out var bottomList))
                    reactionsByNode[bottomId] = bottomList = new List<Reaction>();
                foreach (var rxn in topRxns)
                    bottomList.Add(rxn with { AtNodeId = bottomId });
            }
        }
    }

    private static void AddReaction(Dictionary<string, List<Reaction>> dict, string nodeId, Reaction r)
    {
        if (!dict.TryGetValue(nodeId, out var list))
            dict[nodeId] = list = new List<Reaction>();
        list.Add(r);
    }

    private static double Distance(GraphNode a, GraphNode b)
    {
        double dx = a.X - b.X;
        double dy = a.Y - b.Y;
        double dz = a.Z - b.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
}
