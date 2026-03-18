using StructuralDashboard.Api.Repositories.StructuralModel;

namespace StructuralDashboard.Api.Repositories;

public class StructuralModelRepository : IStructuralModelRepository
{
    private readonly AppDbContext _context;

    public StructuralModelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateModelAsync(StructuralModel model)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteModelAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<StructuralModel>> GetAllModelsAsync(string projectNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<StructuralModel> GetModelAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateModelAsync(StructuralModel model)
    {
        throw new NotImplementedException();
    }

    private async Task<StructuralModel> BuildBaseModelAsync(StructuralModelEntity entity)
    {
        //var beamData = new BeamData(_context);
        //var levelData = new LevelData(_context);
        return new StructuralModel();
    }
}
