namespace StructuralDashboard.Api.Repositories;

public interface IStructuralModelRepository
{
    Task<List<StructuralModel>> GetAllModelsAsync(string projectNumber);
    Task<StructuralModel> GetModelAsync(string id);
    Task CreateModelAsync(StructuralModel model);
    Task DeleteModelAsync(string id);
    Task UpdateModelAsync(StructuralModel model);
}
