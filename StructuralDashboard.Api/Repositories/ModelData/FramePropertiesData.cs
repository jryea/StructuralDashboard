namespace StructuralDashboard.Api.Repositories.ModelData;

public class FramePropertiesData
{
    private readonly AppDbContext _context;

    public FramePropertiesData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FrameProperties>> GetFramePropertiesAsync(string modelId)
    {
        var entities = await _context.FrameProperties
            .Where(fp => fp.ModelId == modelId).ToListAsync();

        var frameProperties = entities.Select(fp => new FrameProperties
        {
            Id = fp.Id,
            Name = fp.Name,
            MaterialId = fp.MaterialId,
        }).ToList();

        return frameProperties;
    }

    public void SaveFrameProperties(string modelId, List<FrameProperties> frameProperties)
    {
        var entities = frameProperties.Select(fp => new FramePropertiesEntity
        {
            Id = fp.Id,
            ModelId = modelId,
            Name = fp.Name,
            MaterialId = fp.MaterialId,
        }).ToList();

        _context.FrameProperties.AddRange(entities);
    }
}
