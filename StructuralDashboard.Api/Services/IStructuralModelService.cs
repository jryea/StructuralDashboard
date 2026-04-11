namespace StructuralDashboard.Api.Services;

public interface IStructuralModelService
{
    Task<List<StructuralModel>> GetAllModelsAsync(string projectNumber);
    Task<StructuralModel?> GetModelAsync(string modelId);
    Task CreateModelAsync(StructuralModel model, string projectNumber);
    Task UpdateModelAsync(StructuralModel model);
    Task DeleteModelAsync(string modelId);
}
