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
            MaterialType = fp.MaterialType,
            ConcreteProps = fp.ConcreteProps is null ? null : new ConcreteFrameProperties
            {
                SectionType = fp.ConcreteProps.SectionType,
                SectionName = fp.ConcreteProps.SectionName,
                Depth = fp.ConcreteProps.Depth,
                Width = fp.ConcreteProps.Width
            },
            SteelProps = fp.SteelProps is null ? null : new SteelFrameProperties
            {
                SectionType = fp.SteelProps.SectionType,
                SectionName = fp.SteelProps.SectionName
            },
            WoodProps = fp.WoodProps is null ? null : new WoodFrameProperties
            {
                SectionType = fp.WoodProps.SectionType,
                SectionName = fp.WoodProps.SectionName,
                Depth = fp.WoodProps.Depth,
                Width = fp.WoodProps.Width
            },
            FrameModifiers = fp.FrameModifiers is null ? new FrameModifiers() : new FrameModifiers
            {
                Area = fp.FrameModifiers.Area,
                A22 = fp.FrameModifiers.A22,
                A33 = fp.FrameModifiers.A33,
                I22 = fp.FrameModifiers.I22,
                I33 = fp.FrameModifiers.I33,
                Torsion = fp.FrameModifiers.Torsion,
                Mass = fp.FrameModifiers.Mass,
                Weight = fp.FrameModifiers.Weight
            }
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
            MaterialType = fp.MaterialType,
            ConcreteProps = fp.ConcreteProps is null ? null : new ConcreteFramePropertiesEntity
            {
                SectionType = fp.ConcreteProps.SectionType,
                SectionName = fp.ConcreteProps.SectionName,
                Depth = fp.ConcreteProps.Depth,
                Width = fp.ConcreteProps.Width
            },
            SteelProps = fp.SteelProps is null ? null : new SteelFramePropertiesEntity
            {
                SectionType = fp.SteelProps.SectionType,
                SectionName = fp.SteelProps.SectionName
            },
            WoodProps = fp.WoodProps is null ? null : new WoodFramePropertiesEntity
            {
                SectionType = fp.WoodProps.SectionType,
                SectionName = fp.WoodProps.SectionName,
                Depth = fp.WoodProps.Depth,
                Width = fp.WoodProps.Width
            },
            FrameModifiers = fp.FrameModifiers is null ? null : new FrameModifiersEntity
            {
                Area = fp.FrameModifiers.Area,
                A22 = fp.FrameModifiers.A22,
                A33 = fp.FrameModifiers.A33,
                I22 = fp.FrameModifiers.I22,
                I33 = fp.FrameModifiers.I33,
                Torsion = fp.FrameModifiers.Torsion,
                Mass = fp.FrameModifiers.Mass,
                Weight = fp.FrameModifiers.Weight
            }
        }).ToList();

        _context.FrameProperties.AddRange(entities);
    }
}