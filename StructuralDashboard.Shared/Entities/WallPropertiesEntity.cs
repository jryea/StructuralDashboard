namespace StructuralDashboard.Shared.Entities;

public class WallPropertiesEntity
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public StructuralModelEntity Model { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string MaterialId { get; set; } = null!;
    public MaterialEntity Material { get; set; } = null!;
    public MaterialType MaterialType { get; set; }
    public double? Thickness { get; set; }
    public double? UnitWeightForSelfWeight { get; set; }
    public ShellModifiersEntity? ETABSModifiers { get; set; }
}