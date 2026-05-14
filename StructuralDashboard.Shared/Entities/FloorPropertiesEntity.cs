namespace StructuralDashboard.Shared.Entities;

public class FloorPropertiesEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public string ModelId { get; set; } = null!;
    public StructuralModelEntity Model { get; set; } = null!;
    public string MaterialId { get; set; } = null!;
    public MaterialEntity Material { get; set; } = null!;
    public StructuralFloorType Type { get; set; }
    public ModelingType ModelingType { get; set; }
    public SlabType SlabType { get; set; }
    public double Thickness { get; set; }
    public DeckPropertiesEntity? DeckProperties { get; set; }
    public ShearStudPropertiesEntity? ShearStudProperties { get; set; }
    public ShellModifiersEntity? ShellModifiers { get; set; }
}