namespace StructuralDashboard.Api.Endpoints;

public static class StructuralModelEndpoints
{
    public static void MapStructuralModelEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/structuralModels");
        // Define model-related endpoints here
    }
}
