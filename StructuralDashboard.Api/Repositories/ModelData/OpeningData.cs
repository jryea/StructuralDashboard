namespace StructuralDashboard.Api.Repositories.ModelData;

public class OpeningData
{
    private readonly AppDbContext _context;

    public OpeningData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Opening>> GetOpeningsAsync(string modelId)
    {
        var entities = await _context.Openings
            .Where(o => o.ModelId == modelId).ToListAsync();

        var openings = entities.Select(o => new Opening
        {
            Id = o.Id,
            LevelId = o.LevelId,
            Points = o.Points
        }).ToList();

        return openings;
    }

    public void SaveOpenings(string modelId, List<Opening> openings)
    {
        var entities = openings.Select(o => new OpeningEntity
        {
            Id = o.Id,
            ModelId = modelId,
            LevelId = o.LevelId,
            Points = o.Points
        }).ToList();

        _context.Openings.AddRange(entities);
    }
}