using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Shared.Contracts;

namespace StructuralDashboard.Api.Services.Loading;

public sealed class LoadingBuilder : ILoadingBuilder
{
    public IReadOnlyList<LoadableBeam> Build(StructuralModel model, StructuralGraph graph)
    {
        // TODO (agent):
        //   For each beam in model.elements.beams:
        //     1. Find its two end nodes in the graph (by matching start/end coordinates).
        //     2. Calculate span from start/end points.
        //     3. Construct a LoadableBeam with:
        //          - MemberId from the beam's id
        //          - StartNodeId, EndNodeId from the matched graph nodes
        //          - SpanFt from the geometric distance
        //          - empty TributaryRegions, IncomingReactions, OutgoingReactions
        //   Return the list.
        //
        // NOTE: walls and columns are NOT built into LoadableBeam — walls are
        // input-only geometry for v1, columns are out of scope until LoadableColumn lands.
        throw new NotImplementedException();
    }
}