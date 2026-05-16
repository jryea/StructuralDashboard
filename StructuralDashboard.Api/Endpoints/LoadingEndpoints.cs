using Microsoft.AspNetCore.Mvc;
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
            [FromServices] IMemberLoadingService service,
            CancellationToken ct) =>
        {
            var result = await service.GetForMemberAsync(modelId, memberId, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapGet("/{modelId}", async (
            string modelId,
            [FromServices] IMemberLoadingService service,
            CancellationToken ct) =>
        {
            try
            {
                var result = await service.GetOrComputeAsync(modelId, ct);
                return Results.Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });
    }
}