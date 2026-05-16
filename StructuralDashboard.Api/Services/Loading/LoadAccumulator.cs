using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Domain.Loading;

namespace StructuralDashboard.Api.Services.Loading;

public sealed class LoadAccumulator : ILoadAccumulator
{
    public IReadOnlyDictionary<string, IReadOnlyList<Reaction>> Run(
        IReadOnlyList<LoadableBeam> beams,
        StructuralGraph graph)
    {
        // TODO (agent):
        //
        // Algorithm — topological pass from top of structure to bottom:
        //
        //   1. Group beams by elevation (their level's Z coordinate). Sort descending.
        //
        //   2. Maintain a Dictionary<string, List<Reaction>> reactionsByNode.
        //
        //   3. For each elevation, from top down:
        //        For each beam at that elevation:
        //          a. Read any IncomingReactions already deposited at this beam's
        //             start/end nodes from reactionsByNode. Attach to beam.IncomingReactions.
        //          b. Compute distributed load (already done via beam.DistributedLoad).
        //          c. Compute end reactions:
        //               R_left + R_right = w * L + Σ point loads
        //               Solve as a simply-supported beam: take moment about one end
        //               to find R_other, then R_first = total - R_other.
        //             For v1, all point loads come from beams above at the end nodes,
        //             so they're applied AT the supports (zero moment arm). This
        //             simplifies the math: each end's reaction is half the distributed
        //             load plus whatever point load arrived at that end.
        //          d. Add OutgoingReactions to beam.OutgoingReactions.
        //          e. Deposit those OutgoingReactions into reactionsByNode at the
        //             corresponding nodes — they'll be picked up by columns or beams
        //             at the level below.
        //
        //   4. Return the final reactionsByNode (converted to IReadOnlyList values).
        //
        // EDGE CASES:
        //   - Joist-on-girder (beams connecting mid-span of other beams via injected
        //     graph nodes): a joist's end node lands on the girder, NOT on a girder's
        //     end node. When the girder is processed, its IncomingReactions includes
        //     the joist's reaction at a non-end node. The simply-supported math needs
        //     to handle this: point load at station x along the span.
        //     Take moments about one end to find the other reaction.
        //   - Beams at the same elevation depending on each other (joist on girder,
        //     both at level 2): process joists BEFORE girders at the same elevation.
        //     The graph's downstream relationship determines order: process a member
        //     only after everything upstream of it has been processed.
        //
        // RECOMMENDED: do a proper topological sort using the graph's upstream/downstream
        // relationships rather than relying solely on elevation grouping. Elevation is a
        // first-pass approximation; the graph order is correct.
        throw new NotImplementedException();
    }
}