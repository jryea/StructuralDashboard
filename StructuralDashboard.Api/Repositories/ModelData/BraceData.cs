namespace StructuralDashboard.Api.Repositories.ModelData;

public class BraceData
{
    private readonly AppDbContext _context;

    public BraceData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Brace>> GetBracesAsync(string modelId)
    {
        var entities = await _context.Braces
            .Where(b => b.ModelId == modelId).ToListAsync();

        var braces = entities.Select(b => new Brace
        {
            Id = b.Id,
            MaterialId = null,
            FramePropertiesId = b.FramePropertiesId,
            BaseLevelId = b.BaseLevelId,
            TopLevelId = b.TopLevelId,
            StartPoint = b.StartPoint,
            EndPoint = b.EndPoint,
            FrameModifiers = b.FrameModifiers is null ? null : new FrameModifiers
            {
                Area = b.FrameModifiers.Area,
                A22 = b.FrameModifiers.A22,
                A33 = b.FrameModifiers.A33,
                I22 = b.FrameModifiers.I22,
                I33 = b.FrameModifiers.I33,
                Torsion = b.FrameModifiers.Torsion,
                Mass = b.FrameModifiers.Mass,
                Weight = b.FrameModifiers.Weight
            }
        }).ToList();

        return braces;
    }

    public void SaveBraces(string modelId, List<Brace> braces, HashSet<string> validFramePropertyIds)
    {
        var entities = braces.Select(b => new BraceEntity
        {
            Id = b.Id,
            ModelId = modelId,
            BaseLevelId = b.BaseLevelId,
            TopLevelId = b.TopLevelId,
            StartPoint = b.StartPoint,
            EndPoint = b.EndPoint,
            FramePropertiesId = validFramePropertyIds.Contains(b.FramePropertiesId ?? "") ? b.FramePropertiesId : null,
            FrameModifiers = b.FrameModifiers is null ? null : new FrameModifiersEntity
            {
                Area = b.FrameModifiers.Area,
                A22 = b.FrameModifiers.A22,
                A33 = b.FrameModifiers.A33,
                I22 = b.FrameModifiers.I22,
                I33 = b.FrameModifiers.I33,
                Torsion = b.FrameModifiers.Torsion,
                Mass = b.FrameModifiers.Mass,
                Weight = b.FrameModifiers.Weight
            }
        }).ToList();

        _context.Braces.AddRange(entities);
    }
}