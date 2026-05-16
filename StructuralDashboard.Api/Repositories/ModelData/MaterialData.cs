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
            DirectionalSymmetryType = m.DirectionalSymmetryType,
            MaterialType = m.MaterialType,
            WeightPerUnitVolume = m.WeightPerUnitVolume,
            MassPerUnitVolume = m.MassPerUnitVolume,
            ElasticModulus = m.ElasticModulus,
            PoissonsRatio = m.PoissonsRatio,
            CoefficientOfThermalExpansion = m.CoefficientOfThermalExpansion,
            ShearModulus = m.ShearModulus,
            ConcreteProps = m.ConcreteProps is null ? null : new ConcreteProperties
            {
                Fc = m.ConcreteProps.Fc,
                WeightClass = m.ConcreteProps.WeightClass,
                ShearStrengthReductionFactor = m.ConcreteProps.ShearStrengthReductionFactor
            },
            SteelProps = m.SteelProps is null ? null : new SteelProperties
            {
                Fy = m.SteelProps.Fy,
                Fu = m.SteelProps.Fu,
                Fye = m.SteelProps.Fye,
                Fue = m.SteelProps.Fue
            }
        }).ToList();

        return materials;
    }

    public void SaveMaterials(string modelId, List<Material> materials)
    {
        var entities = materials.Select(m => new MaterialEntity
        {
            Id = m.Id,
            ModelId = modelId,
            Name = m.Name,
            DirectionalSymmetryType = m.DirectionalSymmetryType,
            MaterialType = m.MaterialType,
            WeightPerUnitVolume = m.WeightPerUnitVolume,
            MassPerUnitVolume = m.MassPerUnitVolume,
            ElasticModulus = m.ElasticModulus,
            PoissonsRatio = m.PoissonsRatio,
            CoefficientOfThermalExpansion = m.CoefficientOfThermalExpansion,
            ShearModulus = m.ShearModulus,
            ConcreteProps = m.ConcreteProps is null ? null : new ConcreteMaterialPropertiesEntity
            {
                Fc = m.ConcreteProps.Fc,
                WeightClass = m.ConcreteProps.WeightClass,
                ShearStrengthReductionFactor = m.ConcreteProps.ShearStrengthReductionFactor
            },
            SteelProps = m.SteelProps is null ? null : new SteelMaterialPropertiesEntity
            {
                Fy = m.SteelProps.Fy,
                Fu = m.SteelProps.Fu,
                Fye = m.SteelProps.Fye,
                Fue = m.SteelProps.Fue
            }
        }).ToList();

        _context.Materials.AddRange(entities);
    }
}