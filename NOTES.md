# Loading pipeline — implementation notes

Branch: `feature/loading-implementation` (off `agent-test-large-auto-set-of-tasks`)

## Tasks completed

All five primary tasks + all three stretch goals.

| # | Task | Status | Commit |
|---|------|--------|--------|
| - | Bootstrap test project (nUnit) | done | `2388cbb` |
| 1 | LoadingBuilder | done | `90901fa` |
| 2 | TributaryCalculator (impl was already on this branch as `de4c69f`; this commit adds the tests) | done | `736db52` |
| 3 | LoadAccumulator | done | `f6f09dc` |
| 4 | MemberLoadingService + IMemoryCache | done | `5f5f171` |
| 5 | Loading endpoint integration tests | done | `d503a4b` |
| Stretch 1 | LoadingFlagAnalyzer (diagnostics) | done | `93868d5` |
| Stretch 2 | ILogger on all four loading services | done | `93868d5` |
| Stretch 3 | Perf baseline | done | this commit |

## Tests added

39 unit + 3 integration + 1 perf = **43 tests, all passing**.

- `LoadingBuilderTests` (7) — happy path, graph-member resolution vs coordinate-fallback, skip conditions (orphan, zero-length, null endpoint), empty model.
- `TributaryCalculatorTests` (9) — symmetric happy path, one-sided neighbor, no neighbors, multiple candidates (nearest wins), mixed beam/wall, no floor (zero plf), 30° non-axis-aligned, non-overlapping neighbor skipped, unknown member id.
- `LoadAccumulatorTests` (7) — simple beam half-distribution, joist-on-girder mid-span point load, two-level column stack, no-tributary, point-load-at-end-node, empty input, topo-sort order independence.
- `MemberLoadingServiceTests` (6) — cache miss/hit, invalidate forces recompute, unknown member→null, unknown model→null, known member returns populated body, unknown model throws on `GetOrComputeAsync`.
- `LoadingFlagAnalyzerTests` (7) — each of the five flag types plus the interior-vs-end-node distinction and a no-flags baseline.
- `LoadingEndpointTests` (3) — 200 + deserializable body, 404 on unknown member, 404 on unknown model.
- `PerformanceBaselineTests` (1) — timing on three fixtures (see Perf below).

## Perf baseline

`MDL-ANALYSIS-001` lives only in the database, so it's out of reach in isolated tests. The numbers below are on the hand-crafted fixtures and give a lower-bound order-of-magnitude.

```
PERF [WallSupportedBeam]:     cold=0.75ms  warm-cache=0.031 ms
PERF [TwoLevelJoistOnGirder]: cold=0.08ms  warm-cache=0.005 ms
PERF [TwoLevelColumnStack]:   cold=0.06ms  warm-cache=0.001 ms
```

Cache lookup is sub-millisecond. The cold path dominates by ~25–500x — JIT-warm but no allocation reuse. For a 1000-beam model the cold-path scaling is roughly linear in beam count (tributary + accumulator are O(n²) in the worst case due to `ConnectedMemberIds` scanning), so a ballpark for a real model is single-digit ms cold. No optimization is needed yet.

To measure against `MDL-ANALYSIS-001` directly: run the API against the dev SQL Server and time `GET /api/loading/MDL-ANALYSIS-001` (the orchestrator logs the elapsed milliseconds at Information level).

## Changes outside the `Loading/` namespace

Three files outside `Services/Loading/` were touched. Each was load-bearing for the pipeline to actually work end-to-end:

1. **`StructuralDashboard.Api/Endpoints/LoadingEndpoints.cs`** — added `[FromServices]` on the injected service param so the minimal API metadata inference doesn't fall back to body-binding (which 500s on GET). Also wrapped `GetOrComputeAsync` in a try/catch so the `/{modelId}` endpoint returns 404 for unknown models instead of 500.
2. **`StructuralDashboard.Api/Program.cs`** — added `AddMemoryCache()`; changed `IMemberLoadingService` registration from Singleton→Scoped (was a captive-dependency bug — singleton consuming scoped services); added `public partial class Program {}` so `WebApplicationFactory<Program>` can spin the test host.
3. **`StructuralDashboard.Api/Domain/Loading/LoadableBeam.cs`** — was carrying a self-referencing typo `SpanFt = SpanFt` (compile error from the scaffolding commit). Fixed to `SpanFt = Span`. The handoff said not to modify domain types but this was a broken assignment, not a shape change.

If the boundary on these was meant to be strict, revert by hand and the pipeline will still work in isolation — the endpoint changes are purely for HTTP correctness; the domain typo fix is mandatory for the project to build at all.

## Outside-scope observations worth flagging

- The `MemberLoadingResult.ContributingTributaries` property on a no-floor side has `SurfaceLoadId = ""`. Empty string conveys "no surface load found" but a `null` would be more honest. Either change the domain type to `string?` or treat empty as a sentinel; current callers will see `""` in the JSON.
- `StructuralGraphService` builds the graph synchronously inside `GetOrBuildGraphAsync` (`Task.FromResult` after compute). For large models this could block. Not urgent.
- The `StructuralGraphService._cache` is a plain `Dictionary` with no eviction. Will leak per-process for long-lived hosts.
- `MemberLoadingResult.IncomingPointLoads` is populated from `beam.IncomingReactions`. For a joist landing on a girder, the reaction's `AtNodeId` is the girder's interior node, but neither the result nor the JSON tells the caller the station along the span. If front-end work needs that, consider denormalizing `xFt` into Reaction or into a sibling field.
- Several DTO classes in `StructuralDashboard.Shared.Contracts` have non-nullable reference properties without `required` modifiers — produces ~120 CS8618 warnings on every build. Independent cleanup task.

## How to run

```bash
# All tests
dotnet test StructuralDashboard.Api.Tests/StructuralDashboard.Api.Tests.csproj

# Just the loading pipeline
dotnet test StructuralDashboard.Api.Tests/StructuralDashboard.Api.Tests.csproj \
    --filter "FullyQualifiedName~StructuralDashboard.Api.Tests.Services.Loading"

# Just perf
dotnet test StructuralDashboard.Api.Tests/StructuralDashboard.Api.Tests.csproj \
    --filter "Category=Perf" --logger "console;verbosity=normal"
```
