using Microsoft.Extensions.Caching.Memory;
using StructuralDashboard.Api.Domain.Graph;
using StructuralDashboard.Api.Services;
using StructuralDashboard.Api.Services.Loading;
using StructuralDashboard.Api.Tests.Fixtures;
using StructuralDashboard.Shared.Contracts;
using System.Diagnostics;

namespace StructuralDashboard.Api.Tests.Services.Loading;

/// <summary>
/// Records GetOrComputeAsync timing on the hand-crafted fixtures. MDL-ANALYSIS-001
/// from the handoff lives only in the database and is out of reach here; the
/// fixture numbers below give a lower-bound order-of-magnitude baseline.
/// Findings get echoed to TestContext.Out and summarised in NOTES.md.
/// </summary>
[TestFixture, Category("Perf")]
public class PerformanceBaselineTests
{
    [Test]
    public async Task GetOrComputeAsync_BaselineTimings_FixtureModels()
    {
        await MeasureAsync("WallSupportedBeam", ModelFixtures.WallSupportedBeam());
        await MeasureAsync("TwoLevelJoistOnGirder", ModelFixtures.TwoLevelJoistOnGirder());
        await MeasureAsync("TwoLevelColumnStack", ModelFixtures.TwoLevelColumnStack());
    }

    private static async Task MeasureAsync(string label, StructuralModel model)
    {
        var modelSvc = new SilentModelService(model);
        var graphSvc = new StructuralGraphService(modelSvc);
        var cache = new MemoryCache(new MemoryCacheOptions());
        var svc = new MemberLoadingService(
            modelSvc, graphSvc, new LoadingBuilder(), new TributaryCalculator(),
            new LoadAccumulator(), cache);

        // Warm JIT
        await svc.GetOrComputeAsync(model.Id);
        svc.Invalidate(model.Id);

        var cold = await TimeOnceAsync(svc, model.Id, invalidate: true);
        var warm = await TimeOnceAsync(svc, model.Id, invalidate: false);

        TestContext.Out.WriteLine(
            $"PERF [{label}]: cold={cold.TotalMilliseconds:F2}ms  warm-cache={warm.TotalMilliseconds:F4}ms");
    }

    private static async Task<TimeSpan> TimeOnceAsync(MemberLoadingService svc, string modelId, bool invalidate)
    {
        if (invalidate) svc.Invalidate(modelId);
        var sw = Stopwatch.StartNew();
        await svc.GetOrComputeAsync(modelId);
        sw.Stop();
        return sw.Elapsed;
    }

    private sealed class SilentModelService : IStructuralModelService
    {
        private readonly StructuralModel _m;
        public SilentModelService(StructuralModel m) => _m = m;
        public Task<StructuralModel?> GetModelAsync(string id) =>
            Task.FromResult<StructuralModel?>(id == _m.Id ? _m : null);
        public Task<List<StructuralModel>> GetAllModelsAsync(string p) => Task.FromResult(new List<StructuralModel> { _m });
        public Task CreateModelAsync(StructuralModel m, string p) => Task.CompletedTask;
        public Task UpdateModelAsync(StructuralModel m) => Task.CompletedTask;
        public Task DeleteModelAsync(string id) => Task.CompletedTask;
    }
}
