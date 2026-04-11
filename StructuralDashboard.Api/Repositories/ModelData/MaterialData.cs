namespace StructuralDashboard.Api.Repositories.ModelData;

public class MaterialData
{
    private readonly AppDbContext _context;
    
    public MaterialData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Material>> GetMaterialsAsync(string modelId)
    {
        var entities = await _context.Materials
            .Where(m => m.ModelId == modelId).ToListAsync();

        var materials = entities.Select(m => new Material
        {
            Id = m.Id,
            Name = m.Name,
            MaterialType = m.MaterialType
        }).ToList();

        return materials;
    }

    public void SaveMaterials(string modelId, List<Material> materials)
    {
        // Convert materials to entities
        var entities = materials.Select(m => new MaterialEntity
        {
            Id = m.Id,
            ModelId = modelId,
            Name = m.Name,
            MaterialType = m.MaterialType
        }).ToList();

        _context.Materials.AddRange(entities);
    }
}
