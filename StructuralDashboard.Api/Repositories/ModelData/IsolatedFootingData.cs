namespace StructuralDashboard.Api.Repositories.ModelData;

public class IsolatedFootingData
{
    private readonly AppDbContext _context;

    public IsolatedFootingData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<IsolatedFooting>> GetIsolatedFootingsAsync(string modelId)
    {
        var entities = await _context.Footings
            .Where(f => f.ModelId == modelId).ToListAsync();

        var footings = entities.Select(f => new IsolatedFooting
        {
            Id = f.Id,
            Width = f.Width,
            Length = f.Length,
            Thickness = f.Thickness,
            Point = f.Point,
            LevelId = f.LevelId,
            Orientation = f.Orientation
        }).ToList();

        return footings;
    }

    public void SaveIsolatedFootings(string modelId, List<IsolatedFooting> footings)
    {
        var entities = footings.Select(f => new IsolatedFootingEntity
        {
            Id = f.Id,
            ModelId = modelId,
            Width = f.Width,
            Length = f.Length,
            Thickness = f.Thickness,
            Point = f.Point,
            LevelId = f.LevelId,
            Orientation = f.Orientation
        }).ToList();

        _context.Footings.AddRange(entities);
    }
}