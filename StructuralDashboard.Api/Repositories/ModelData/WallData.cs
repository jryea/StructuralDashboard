namespace StructuralDashboard.Api.Repositories.ModelData;

public class WallData
{
    private readonly AppDbContext _context;

    public WallData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Wall>> GetWallsAsync(string modelId)
    {
        var entities = await _context.Walls
            .Where(w => w.ModelId == modelId).ToListAsync();

        var walls = entities.Select(w => new Wall
        {
            Id = w.Id,
            StartPoint = w.StartPoint,
            EndPoint = w.EndPoint,
            BaseLevelId = w.BaseLevelId,
            TopLevelId = w.TopLevelId,
            PropertiesId = w.PropertiesId,
            IsLateral = w.IsLateral
        }).ToList();

        return walls;
    }

    public void SaveWalls(string modelId, List<Wall> walls, HashSet<string> validWallPropertyIds)
    {
        var entities = walls.Select(w => new WallEntity
        {
            Id = w.Id,
            ModelId = modelId,
            StartPoint = w.StartPoint,
            EndPoint = w.EndPoint,
            BaseLevelId = w.BaseLevelId,
            TopLevelId = w.TopLevelId,
            PropertiesId = validWallPropertyIds.Contains(w.PropertiesId ?? "") ? w.PropertiesId : null,
            IsLateral = w.IsLateral
        }).ToList();

        _context.Walls.AddRange(entities);
    }
}