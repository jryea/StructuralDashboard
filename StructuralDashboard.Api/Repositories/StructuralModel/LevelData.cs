namespace StructuralDashboard.Api.Repositories.StructuralModel;

public class LevelData
{
    private readonly AppDbContext _context;

    public LevelData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Level>> GetLevelsAsync(string modelId)
    {
        var entities = await _context.Levels
            .Where(l => l.ModelId == modelId).ToListAsync();

        var levels = entities.Select(l => new Level
        {
            Id = l.Id,
            Name = l.Name,
            Elevation = l.Elevation
        }).ToList();

        return levels;
    }

    public async Task SaveLevelsAsync(string modelId, List<Level> levels)
    {
        var entities = levels.Select(l => new LevelEntity
        {
            Id = l.Id,
            ModelId = modelId,
            Name = l.Name,
            Elevation = l.Elevation
        });

        _context.Levels.AddRange(entities);
    }
}
