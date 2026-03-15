using StructuralDashboard.Shared.Contracts.Enums;
using System.Security.Principal;

namespace StructuralDashboard.Shared.Entities;

public class Material
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public string ModelId { get; set; } = null!;
    public Model Model { get; set; } = null!;
    public DirectionalSymmetryType? DirectionalSymmetryType { get; set; }
    public MaterialType? MaterialType { get; set; }
    public double? WeightPerUnitVolume { get; set; }
    public double? MassPerUnitVolume { get; set; }  
    public double? ElasticModulus { get; set; }
    public double? PoissonsRatio { get; set; }
    public double? CoefficientOfThermalExpansion { get; set; }
    public double? ShearModulus { get; set; }   
}