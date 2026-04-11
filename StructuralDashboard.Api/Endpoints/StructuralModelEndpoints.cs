namespace StructuralDashboard.Api.Endpoints;

public static class StructuralModelEndpoints
{
    public static void MapStructuralModelEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/structuralModels");

        group.MapGet("/project/{projectNumber}", GetAllStructuralModels);
        group.MapGet("/{modelId}", GetStructuralModel);
        group.MapPost("/", CreateStructuralModel);
        group.MapPut("/{modelId}", UpdateStructuralModel);
        group.MapDelete("/{modelId}", DeleteModel);
    }

    private static async Task<IResult> GetAllStructuralModels(IStructuralModelService modelService, string projectNumber)
    {
        var models = await modelService.GetAllModelsAsync(projectNumber);
        return Results.Ok(models);
    }
    private static async Task<IResult> GetStructuralModel(IStructuralModelService modelService, string modelId)
    {
        var model = await modelService.GetModelAsync(modelId);

        if (model != null)
        {
            return Results.Ok(model);
        }

        return Results.NotFound();
    }

    private static async Task<IResult> CreateStructuralModel(IStructuralModelService modelService, StructuralModel model, string projectNumber)
    {
        await modelService.CreateModelAsync(model, projectNumber);
        return Results.Created($"/api/projects/{model.Id}", model);
    }

    private static async Task<IResult> UpdateStructuralModel(IStructuralModelService modelService, StructuralModel model)
    {
        await modelService.UpdateModelAsync(model);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteModel(IStructuralModelService modelService, string modelId)
    {
        await modelService.DeleteModelAsync(modelId);
        return Results.NoContent();
    }
}
