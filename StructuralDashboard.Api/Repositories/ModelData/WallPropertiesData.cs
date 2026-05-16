namespace StructuralDashboard.Api.Repositories.ModelData;

public class WallPropertiesData
{
    private readonly AppDbContext _context;

    public WallPropertiesData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WallProperties>> GetWallPropertiesAsync(string modelId)
    {
        var entities = await _context.WallProperties
            .Where(wp => wp.ModelId == modelId).ToListAsync();

        var wallProperties = entities.Select(wp => new WallProperties
        {
            Id = wp.Id,
            Name = wp.Name,
            MaterialId = wp.MaterialId,
            MaterialType = wp.MaterialType,
            Thickness = wp.Thickness,
            UnitWeightForSelfWeight = wp.UnitWeightForSelfWeight,
            ETABSModifiers = wp.ETABSModifiers is null ? null : new ShellModifiers
            {
                F11 = wp.ETABSModifiers.F11,
                F22 = wp.ETABSModifiers.F22,
                F12 = wp.ETABSModifiers.F12,
                M11 = wp.ETABSModifiers.M11,
                M22 = wp.ETABSModifiers.M22,
                M12 = wp.ETABSModifiers.M12,
                V13 = wp.ETABSModifiers.V13,
                V23 = wp.ETABSModifiers.V23,
                Mass = wp.ETABSModifiers.Mass,
                Weight = wp.ETABSModifiers.Weight
            }
        }).ToList();

        return wallProperties;
    }

    public void SaveWallProperties(string modelId, List<WallProperties> wallProperties)
    {
        var entities = wallProperties.Select(wp => new WallPropertiesEntity
        {
            Id = wp.Id,
            ModelId = modelId,
            Name = wp.Name,
            MaterialId = wp.MaterialId,
            MaterialType = wp.MaterialType,
            Thickness = wp.Thickness,
            UnitWeightForSelfWeight = wp.UnitWeightForSelfWeight,
            ETABSModifiers = wp.ETABSModifiers is null ? null : new ShellModifiersEntity
            {
                F11 = wp.ETABSModifiers.F11,
                F22 = wp.ETABSModifiers.F22,
                F12 = wp.ETABSModifiers.F12,
                M11 = wp.ETABSModifiers.M11,
                M22 = wp.ETABSModifiers.M22,
                M12 = wp.ETABSModifiers.M12,
                V13 = wp.ETABSModifiers.V13,
                V23 = wp.ETABSModifiers.V23,
                Mass = wp.ETABSModifiers.Mass,
                Weight = wp.ETABSModifiers.Weight
            }
        }).ToList();

        _context.WallProperties.AddRange(entities);
    }
}