namespace StructuralDashboard.Api.Endpoints;

public static class ModelEndpoints
{
    public static void MapModelEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/models");
        // Define model-related endpoints here
    }
}
