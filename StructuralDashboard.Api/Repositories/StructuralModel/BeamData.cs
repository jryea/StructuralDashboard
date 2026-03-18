namespace StructuralDashboard.Api.Repositories.StructuralModel;

public class BeamData
{
    private readonly AppDbContext _context;

    public BeamData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Beam>> GetBeamsAsync(string modelId)
    {
        var entities = await _context.Beams
            .Where(b => b.ModelId == modelId).ToListAsync();

        var beams = entities.Select(b => new Beam
        {
            Id = b.Id,
            LevelId = b.LevelId,
            StartPoint = b.StartPoint,
            EndPoint = b.EndPoint,
            IsJoist = b.IsJoist,
            IsLateral = b.IsLateral,
            FramePropertiesId = b.FramePropertiesId
        }).ToList();

        return beams;
    }

    public async Task SaveBeamsAsync(string modelId, List<Beam> beams)
    {
        var entities = beams.Select(b => new BeamEntity
        {
            Id = b.Id,
            ModelId = modelId,
            LevelId = b.LevelId,
            StartPoint = b.StartPoint,
            EndPoint = b.EndPoint,
            IsJoist = b.IsJoist,
            IsLateral = b.IsLateral,
            FramePropertiesId = b.FramePropertiesId
        });

        _context.Beams.AddRange(entities);
    }
}
