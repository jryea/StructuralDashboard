using StructuralDashboard.Shared.Contracts.Enums;

namespace StructuralDashboard.Shared.Entities;

public class FloorProperties
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public string ModelId { get; set; } = null!; 
    public Model Model { get; set; } = null!;   

    public string MaterialId { get; set; } = null!;
    public Material Material { get; set; } = null!;
    public StructuralFloorType Type { get; set; }
    public ModelingType ModelingType { get; set; }
    public SlabType SlabType { get; set; }  
    public double Thickness { get; set; }
    public string DeckPropertiesId { get; set; } = null!;        
    public DeckProperties? DeckProperties { get; set; } = null;
    public ShearStudProperties? ShearStudProperties { get; set; } = null;
    public ShellModifiers? ShellModifiers { get; set; } = null;  
}