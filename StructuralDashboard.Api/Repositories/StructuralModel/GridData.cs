namespace StructuralDashboard.Api.Repositories.StructuralModel;

public class GridData
{
    private readonly AppDbContext _context;

    public GridData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Grid>> GetGridsAsync(string modelId)
    {
        var entities = await _context.Grids
            .Where(g => g.ModelId == modelId).ToListAsync();

        var grids = entities.Select(g => new Grid
        {
            Id = g.Id,
            Name = g.Name,
            StartPoint = g.StartPoint,
            EndPoint = g.EndPoint
        }).ToList();

        return grids;
    }

    public async Task SaveGridsAsync(string modelId, List<Grid> grids)
    {
        var entities = grids.Select(g => new GridEntity
        {
            Id = g.Id,
            ModelId = modelId,
            Name = g.Name,
            StartPoint = g.StartPoint,
            EndPoint = g.EndPoint
        });

        _context.Grids.AddRange(entities);
    }
}
