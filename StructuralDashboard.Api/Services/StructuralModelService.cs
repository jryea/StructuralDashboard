namespace StructuralDashboard.Api.Services;

public class StructuralModelService : IStructuralModelService
{
    private readonly IStructuralModelRepository _repository;

    public StructuralModelService(IStructuralModelRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateModelAsync(StructuralModel model, string projectNumber)
    {
        await _repository.CreateModelAsync(model, projectNumber);
    }

    public async Task DeleteModelAsync(string modelId)
    {
        await _repository.DeleteModelAsync(modelId);
    }

    public async Task<List<StructuralModel>> GetAllModelsAsync(string projectNumber)
    {
        return await _repository.GetAllModelsAsync(projectNumber);
    }

    public async Task<StructuralModel?> GetModelAsync(string modelId)
    {
        return await _repository.GetModelAsync(modelId);
    }

    public async Task UpdateModelAsync(StructuralModel model)
    {
        await _repository.UpdateModelAsync(model);
    }
}
