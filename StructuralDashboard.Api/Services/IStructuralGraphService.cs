using StructuralDashboard.Api.Domain;
using StructuralDashboard.Shared.Contracts;

namespace StructuralDashboard.Api.Services;

public interface IStructuralGraphService
{
    // Check cache first, build from model if not found
    Task<StructuralGraph> GetOrBuildGraphAsync(string modelId);

    // Traverse the cached graph and return the load path
    Task<LoadPath> GetLoadPathAsync(string modelId, string memberId);
}