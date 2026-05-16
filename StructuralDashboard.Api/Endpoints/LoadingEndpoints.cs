using StructuralDashboard.Api.Services.Loading;
using System.Threading;

public static class LoadingEndpoints
{
    public static void MapLoadingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/loading");

        group.MapGet("/{modelId}/members/{memberId}", async (
            string modelId,
            string memberId,
            IMemberLoadingService service,
            CancellationToken ct) =>
        {
            var result = await service.GetForMemberAsync(modelId, memberId, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapGet("/{modelId}", async (
            string modelId,
            IMemberLoadingService service,
            CancellationToken ct) =>
        {
            var result = await service.GetOrComputeAsync(modelId, ct);
            return Results.Ok(result);
        });
    }
}