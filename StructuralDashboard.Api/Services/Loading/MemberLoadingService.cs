using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _cache;

    public MemberLoadingService(
        IStructuralModelService models,
        IStructuralGraphService graphs,
        ILoadingBuilder builder,
        ITributaryCalculator tributaryCalculator,
        ILoadAccumulator accumulator,
        IMemoryCache cache)
    {
        _models = models;
        _graphs = graphs;
        _builder = builder;
        _tributaryCalculator = tributaryCalculator;
        _accumulator = accumulator;
        _cache = cache;
    }

    public async Task<ModelLoadingResult> GetOrComputeAsync(string modelId, CancellationToken ct = default)
    {
        if (_cache.TryGetValue(CacheKey(modelId), out ModelLoadingResult? cached) && cached is not null)
            return cached;

        var model = await _models.GetModelAsync(modelId)
            ?? throw new KeyNotFoundException($"Model {modelId} not found");

        var graph = await _graphs.GetOrBuildGraphAsync(modelId);

        var beams = _builder.Build(model, graph);
        foreach (var beam in beams)
            foreach (var trib in _tributaryCalculator.CalculateForBeam(beam, model))
                beam.Tributaries.Add(trib);

        var reactionsByNode = _accumulator.Run(beams, graph);

        var members = beams.ToDictionary(b => b.MemberId, b => b.ToResult());

        var result = new ModelLoadingResult
        {
            ModelId = modelId,
            ComputedAt = DateTime.UtcNow,
            Members = members,
            ReactionsByNode = reactionsByNode
        };

        _cache.Set(CacheKey(modelId), result);
        return result;
    }

    public async Task<MemberLoadingResult?> GetForMemberAsync(string modelId, string memberId, CancellationToken ct = default)
    {
        ModelLoadingResult model;
        try
        {
            model = await GetOrComputeAsync(modelId, ct);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
        return model.Members.TryGetValue(memberId, out var result) ? result : null;
    }

    public void Invalidate(string modelId) => _cache.Remove(CacheKey(modelId));

    private static string CacheKey(string modelId) => $"loading:{modelId}";
}
