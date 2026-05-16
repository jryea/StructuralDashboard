using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Domain.Loading;
using StructuralDashboard.Shared.Contracts;

namespace StructuralDashboard.Api.Services.Loading;

public interface ILoadingBuilder
{
    /// <summary>
    /// Constructs empty LoadableBeam instances from a structural model and its graph.
    /// Tributaries and reactions are populated by later services in the pipeline.
    /// </summary>
    IReadOnlyList<LoadableBeam> Build(StructuralModel model, StructuralGraph graph);
}