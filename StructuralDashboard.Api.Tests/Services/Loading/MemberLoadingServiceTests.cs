using Microsoft.Extensions.Caching.Memory;
using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Services;
using StructuralDashboard.Api.Services.Loading;
using StructuralDashboard.Api.Tests.Fixtures;
using StructuralDashboard.Shared.Contracts;

namespace StructuralDashboard.Api.Tests.Services.Loading;

[TestFixture]
public class MemberLoadingServiceTests
{
    private static (MemberLoadingService svc, CountingModelService modelSvc, CountingGraphService graphSvc) Build(StructuralModel model)
    {
        var modelSvc = new CountingModelService(model);
        var graphSvc = new CountingGraphService(modelSvc);
        var cache = new MemoryCache(new MemoryCacheOptions());
        var svc = new MemberLoadingService(
            modelSvc,
            graphSvc,
            new LoadingBuilder(),
            new TributaryCalculator(),
            new LoadAccumulator(),
            cache);
        return (svc, modelSvc, graphSvc);
    }

    [Test]
    public async Task GetOrComputeAsync_FirstCallComputes_SecondCallReturnsFromCache()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var (svc, modelSvc, graphSvc) = Build(model);

        var first = await svc.GetOrComputeAsync(model.Id);
        var second = await svc.GetOrComputeAsync(model.Id);

        Assert.That(ReferenceEquals(first, second), Is.True,
            "Second call must return the cached instance, not a recompute.");
        Assert.That(modelSvc.GetModelCallCount, Is.EqualTo(1),
            "Model fetch must only happen on the cache miss.");
        Assert.That(graphSvc.GetOrBuildCallCount, Is.EqualTo(1),
            "Graph build must only happen on the cache miss.");
    }

    [Test]
    public async Task Invalidate_CausesRecomputeOnNextCall()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var (svc, modelSvc, _) = Build(model);

        await svc.GetOrComputeAsync(model.Id);
        svc.Invalidate(model.Id);
        await svc.GetOrComputeAsync(model.Id);

        Assert.That(modelSvc.GetModelCallCount, Is.EqualTo(2));
    }

    [Test]
    public async Task GetForMemberAsync_UnknownMember_ReturnsNullWithoutThrowing()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var (svc, _, _) = Build(model);

        var result = await svc.GetForMemberAsync(model.Id, "does-not-exist");
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetForMemberAsync_UnknownModel_ReturnsNullWithoutThrowing()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var (svc, _, _) = Build(model);

        var result = await svc.GetForMemberAsync("MDL-DOES-NOT-EXIST", "B1");
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetForMemberAsync_KnownMember_ReturnsResultWithLoading()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var (svc, _, _) = Build(model);

        var result = await svc.GetForMemberAsync(model.Id, "B1");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.MemberId, Is.EqualTo("B1"));
        Assert.That(result.SpanFt, Is.EqualTo(20.0).Within(1e-6));
        // The wall-supported beam: 50 + 50 psf × 2.5 ft × 2 sides = 500 plf
        Assert.That(result.DistributedLoad, Is.EqualTo(500).Within(1e-3));
        Assert.That(result.ContributingTributaries, Is.Not.Empty);
        Assert.That(result.EndReactions, Has.Count.EqualTo(2));
    }

    [Test]
    public void GetOrComputeAsync_UnknownModel_ThrowsKeyNotFound()
    {
        var model = ModelFixtures.WallSupportedBeam();
        var (svc, _, _) = Build(model);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await svc.GetOrComputeAsync("MDL-DOES-NOT-EXIST"));
    }

    // --- helpers ---

    private sealed class CountingModelService : IStructuralModelService
    {
        internal readonly StructuralModel _model;
        public int GetModelCallCount { get; private set; }
        public CountingModelService(StructuralModel model) => _model = model;
        public Task<StructuralModel?> GetModelAsync(string modelId)
        {
            GetModelCallCount++;
            return Task.FromResult<StructuralModel?>(modelId == _model.Id ? _model : null);
        }
        internal StructuralModel? PeekModel(string modelId) =>
            modelId == _model.Id ? _model : null;
        public Task<List<StructuralModel>> GetAllModelsAsync(string p) => Task.FromResult(new List<StructuralModel> { _model });
        public Task CreateModelAsync(StructuralModel m, string p) => Task.CompletedTask;
        public Task UpdateModelAsync(StructuralModel m) => Task.CompletedTask;
        public Task DeleteModelAsync(string id) => Task.CompletedTask;
    }

    /// <summary>
    /// Pre-builds the graph from the fixture model so this stub doesn't re-call
    /// the counting model service (which would skew GetModelCallCount).
    /// </summary>
    private sealed class CountingGraphService : IStructuralGraphService
    {
        private readonly StructuralGraph _graph;
        public int GetOrBuildCallCount { get; private set; }
        public CountingGraphService(IStructuralModelService models)
        {
            // Build the graph once with a non-counting stub, so the call count we expose
            // reflects only what MemberLoadingService asks for.
            _graph = new StructuralGraphService(new SilentModelProxy(models))
                .GetOrBuildGraphAsync(((CountingModelService)models)._model.Id)
                .GetAwaiter().GetResult();
        }
        public Task<StructuralGraph> GetOrBuildGraphAsync(string modelId)
        {
            GetOrBuildCallCount++;
            return Task.FromResult(_graph);
        }
        public Task<LoadPath> GetLoadPathAsync(string modelId, string memberId) => throw new NotImplementedException();
    }

    private sealed class SilentModelProxy : IStructuralModelService
    {
        private readonly IStructuralModelService _inner;
        public SilentModelProxy(IStructuralModelService inner) => _inner = inner;
        public Task<StructuralModel?> GetModelAsync(string modelId) =>
            Task.FromResult(((CountingModelService)_inner).PeekModel(modelId));
        public Task<List<StructuralModel>> GetAllModelsAsync(string p) => _inner.GetAllModelsAsync(p);
        public Task CreateModelAsync(StructuralModel m, string p) => Task.CompletedTask;
        public Task UpdateModelAsync(StructuralModel m) => Task.CompletedTask;
        public Task DeleteModelAsync(string id) => Task.CompletedTask;
    }
}
