using StructuralDashboard.Api.Domain.Loading;
using System.Threading;

namespace StructuralDashboard.Api.Services.Loading;

public interface IMemberLoadingService
{
    /// <summary>
    /// Returns the cached loading result for a model, computing it if not cached.
    /// </summary>
    Task<ModelLoadingResult> GetOrComputeAsync(string modelId, CancellationToken ct = default);

    /// <summary>
    /// Returns the loading result for a single member within a model.
    /// </summary>
    Task<MemberLoadingResult?> GetForMemberAsync(string modelId, string memberId, CancellationToken ct = default);

    /// <summary>
    /// Invalidates the cached result for a model, forcing recomputation on the next call.
    /// </summary>
    void Invalidate(string modelId);
}