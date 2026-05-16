using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Domain.Loading;

namespace StructuralDashboard.Api.Services.Loading;

public interface ILoadAccumulator
{
    /// <summary>
    /// Runs the top-to-bottom load accumulation pass. Mutates the provided
    /// LoadableBeam instances by filling in IncomingReactions and OutgoingReactions
    /// based on the graph topology. Tributaries must already be populated.
    /// </summary>
    /// <returns>A node-keyed reactions dictionary for downstream lookups.</returns>
    IReadOnlyDictionary<string, IReadOnlyList<Reaction>> Run(
        IReadOnlyList<LoadableBeam> beams,
        StructuralGraph graph);
}