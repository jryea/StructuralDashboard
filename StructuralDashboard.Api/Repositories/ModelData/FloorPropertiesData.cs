namespace StructuralDashboard.Api.Repositories.ModelData;

public class FloorPropertiesData
{
    private readonly AppDbContext _context;

    public FloorPropertiesData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FloorProperties>> GetFloorPropertiesAsync(string modelId)
    {
        var entities = await _context.FloorProperties
            .Where(fp => fp.ModelId == modelId).ToListAsync();

        var floorProperties = entities.Select(fp => new FloorProperties
        {
            Id = fp.Id,
            Name = fp.Name,
            MaterialId = fp.MaterialId,
            Type = fp.Type,
            ModelingType = fp.ModelingType,
            SlabType = fp.SlabType,
            Thickness = fp.Thickness,
            DeckProperties = fp.DeckProperties is null ? null : new DeckProperties
            {
                DeckType = fp.DeckProperties.DeckType,
                MaterialID = fp.DeckProperties.MaterialId ?? "",
                RibDepth = fp.DeckProperties.RibDepth,
                RibWidthTop = fp.DeckProperties.RibWidthTop,
                RibWidthBottom = fp.DeckProperties.RibWidthBottom,
                RibSpacing = fp.DeckProperties.RibSpacing,
                DeckShearThickness = fp.DeckProperties.DeckShearThickness,
                DeckUnitWeight = fp.DeckProperties.DeckUnitWeight
            },
            ShearStudProperties = fp.ShearStudProperties is null ? null : new ShearStudProperties
            {
                ShearStudDiameter = fp.ShearStudProperties.ShearStudDiameter ?? 0,
                ShearStudHeight = fp.ShearStudProperties.ShearStudHeight ?? 0,
                ShearStudTensileStrength = fp.ShearStudProperties.ShearStudTensileStrength ?? 0
            },
            ShellModifiers = fp.ShellModifiers is null ? new ShellModifiers() : new ShellModifiers
            {
                F11 = fp.ShellModifiers.F11,
                F22 = fp.ShellModifiers.F22,
                F12 = fp.ShellModifiers.F12,
                M11 = fp.ShellModifiers.M11,
                M22 = fp.ShellModifiers.M22,
                M12 = fp.ShellModifiers.M12,
                V13 = fp.ShellModifiers.V13,
                V23 = fp.ShellModifiers.V23,
                Mass = fp.ShellModifiers.Mass,
                Weight = fp.ShellModifiers.Weight
            }
        }).ToList();

        return floorProperties;
    }

    public void SaveFloorProperties(string modelId, List<FloorProperties> floorProperties)
    {
        var entities = floorProperties.Select(fp => new FloorPropertiesEntity
        {
            Id = fp.Id,
            ModelId = modelId,
            Name = fp.Name,
            MaterialId = fp.MaterialId,
            Type = fp.Type,
            ModelingType = fp.ModelingType,
            SlabType = fp.SlabType,
            Thickness = fp.Thickness,
            DeckProperties = fp.DeckProperties is null ? null : new DeckPropertiesEntity
            {
                DeckType = fp.DeckProperties.DeckType,
                MaterialId = fp.DeckProperties.MaterialID,
                RibDepth = fp.DeckProperties.RibDepth,
                RibWidthTop = fp.DeckProperties.RibWidthTop,
                RibWidthBottom = fp.DeckProperties.RibWidthBottom,
                RibSpacing = fp.DeckProperties.RibSpacing,
                DeckShearThickness = fp.DeckProperties.DeckShearThickness,
                DeckUnitWeight = fp.DeckProperties.DeckUnitWeight
            },
            ShearStudProperties = fp.ShearStudProperties is null ? null : new ShearStudPropertiesEntity
            {
                ShearStudDiameter = fp.ShearStudProperties.ShearStudDiameter,
                ShearStudHeight = fp.ShearStudProperties.ShearStudHeight,
                ShearStudTensileStrength = fp.ShearStudProperties.ShearStudTensileStrength
            },
            ShellModifiers = fp.ShellModifiers is null ? null : new ShellModifiersEntity
            {
                F11 = fp.ShellModifiers.F11,
                F22 = fp.ShellModifiers.F22,
                F12 = fp.ShellModifiers.F12,
                M11 = fp.ShellModifiers.M11,
                M22 = fp.ShellModifiers.M22,
                M12 = fp.ShellModifiers.M12,
                V13 = fp.ShellModifiers.V13,
                V23 = fp.ShellModifiers.V23,
                Mass = fp.ShellModifiers.Mass,
                Weight = fp.ShellModifiers.Weight
            }
        }).ToList();

        _context.FloorProperties.AddRange(entities);
    }
}