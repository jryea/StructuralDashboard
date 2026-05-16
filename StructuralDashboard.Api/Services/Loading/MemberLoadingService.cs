using StructuralDashboard.Api.Domain.Loading;
using System.Threading;

namespace StructuralDashboard.Api.Services.Loading;

public sealed class MemberLoadingService : IMemberLoadingService
{
    private readonly IStructuralModelService _models;
    private readonly IStructuralGraphService _graphs;
    private readonly ILoadingBuilder _builder;
    private readonly ITributaryCalculator _tributaryCalculator;
    private readonly ILoadAccumulator _accumulator;
    // TODO (agent): inject a caching abstraction (IMemoryCache for v1)

    public MemberLoadingService(
        IStructuralModelService models,
        IStructuralGraphService graphs,
        ILoadingBuilder builder,
        ITributaryCalculator tributaryCalculator,
        ILoadAccumulator accumulator)
    {
        _models = models;
        _graphs = graphs;
        _builder = builder;
        _tributaryCalculator = tributaryCalculator;
        _accumulator = accumulator;
    }

    public Task<ModelLoadingResult> GetOrComputeAsync(string modelId, CancellationToken ct = default)
    {
        // TODO (agent):
        //   1. Check cache for modelId. If present, return.
        //   2. Fetch the structural model via _models.
        //   3. Fetch the structural graph via _graphs.
        //   4. Call _builder.Build(model, graph) → list of LoadableBeam (empty tributaries/reactions).
        //   5. For each beam, _tributaryCalculator.CalculateForBeam(beam, model) → populate tributaries.
        //   6. _accumulator.Run(beams, graph) → fills distributed load, reactions, propagates.
        //   7. Project beams → MemberLoadingResults, build ReactionsByNode dictionary.
        //   8. Assemble ModelLoadingResult, store in cache, return.
        throw new NotImplementedException();
    }

    public async Task<MemberLoadingResult?> GetForMemberAsync(string modelId, string memberId, CancellationToken ct = default)
    {
        var model = await GetOrComputeAsync(modelId, ct);
        return model.Members.TryGetValue(memberId, out var result) ? result : null;
    }

    public void Invalidate(string modelId)
    {
        // TODO (agent): remove from cache.
    }
}