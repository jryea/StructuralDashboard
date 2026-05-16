namespace StructuralDashboard.Shared.Entities;

public class MaterialEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public string ModelId { get; set; } = null!;
    public StructuralModelEntity Model { get; set; } = null!;
    public DirectionalSymmetryType? DirectionalSymmetryType { get; set; }
    public MaterialType? MaterialType { get; set; }
    public double? WeightPerUnitVolume { get; set; }
    public double? MassPerUnitVolume { get; set; }
    public double? ElasticModulus { get; set; }
    public double? PoissonsRatio { get; set; }
    public double? CoefficientOfThermalExpansion { get; set; }
    public double? ShearModulus { get; set; }
    public ConcreteMaterialPropertiesEntity? ConcreteProps { get; set; }
    public SteelMaterialPropertiesEntity? SteelProps { get; set; }
}

public class ConcreteMaterialPropertiesEntity
{
    public double? Fc { get; set; }
    public WeightClass? WeightClass { get; set; }
    public double? ShearStrengthReductionFactor { get; set; }
}

public class SteelMaterialPropertiesEntity
{
    public double? Fy { get; set; }
    public double? Fu { get; set; }
    public double? Fye { get; set; }
    public double? Fue { get; set; }
}