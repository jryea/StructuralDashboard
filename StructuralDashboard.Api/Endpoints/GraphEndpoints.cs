namespace StructuralDashboard.Api.Endpoints;

public static class GraphEndpoints
{
    public static void MapGraphEndpoints(this WebApplication app)
    {
        // Groups endpoints in Swagger UI — all graph endpoints appear under the "Graph" section
        var group = app.MapGroup("/api/graph")
            .WithTags("Graph");

        // Short description shown next to the endpoint in Swagger UI
        group.MapGet("/models/{modelId}/loadpath", GetLoadPath)
            .WithSummary("Get the load path for a selected member");
    }

    private static async Task<IResult> GetLoadPath(
        string modelId,
        string memberId,
        IStructuralGraphService graphService)
    {
        try
        {
            var loadPath = await graphService.GetLoadPathAsync(modelId, memberId);
            return Results.Ok(loadPath);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}