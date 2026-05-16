using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Shared.Contracts;

namespace StructuralDashboard.Api.Services.Loading;

public sealed class LoadingBuilder : ILoadingBuilder
{
    private const double InchesPerFoot = 12.0;
    private const double NodeMatchToleranceInches = 0.1;

    public IReadOnlyList<LoadableBeam> Build(StructuralModel model, StructuralGraph graph)
    {
        var loadables = new List<LoadableBeam>();
        if (model?.Elements?.Beams is null) return loadables;

        foreach (var beam in model.Elements.Beams)
        {
            if (beam is null || string.IsNullOrEmpty(beam.Id)) continue;
            if (beam.StartPoint is null || beam.EndPoint is null) continue;

            var endpoints = ResolveEndpoints(beam, graph);
            if (endpoints is null) continue;

            double spanFeet = Distance(beam.StartPoint, beam.EndPoint) / InchesPerFoot;
            if (spanFeet <= 0) continue;

            loadables.Add(new LoadableBeam
            {
                MemberId = beam.Id,
                StartNodeId = endpoints.Value.start,
                EndNodeId = endpoints.Value.end,
                Span = spanFeet
            });
        }

        return loadables;
    }

    private static (string start, string end)? ResolveEndpoints(Beam beam, StructuralGraph graph)
    {
        if (graph?.Members is not null &&
            graph.Members.TryGetValue(beam.Id, out var graphMember) &&
            graphMember.NodeIds.Count >= 2)
        {
            return (graphMember.NodeIds[0], graphMember.NodeIds[^1]);
        }

        if (graph?.Nodes is null) return null;

        var startId = FindNodeByCoordinate(graph, beam.StartPoint);
        var endId = FindNodeByCoordinate(graph, beam.EndPoint);
        if (startId is null || endId is null) return null;
        return (startId, endId);
    }

    private static string? FindNodeByCoordinate(StructuralGraph graph, Point p)
    {
        string? bestId = null;
        double bestDistSq = NodeMatchToleranceInches * NodeMatchToleranceInches;
        foreach (var node in graph.Nodes.Values)
        {
            double dx = node.X - p.X;
            double dy = node.Y - p.Y;
            double dz = node.Z - p.Z;
            double d2 = dx * dx + dy * dy + dz * dz;
            if (d2 <= bestDistSq)
            {
                bestDistSq = d2;
                bestId = node.Id;
            }
        }
        return bestId;
    }

    private static double Distance(Point a, Point b)
    {
        double dx = a.X - b.X;
        double dy = a.Y - b.Y;
        double dz = a.Z - b.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
}
