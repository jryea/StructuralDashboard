using StructuralDashboard.Shared.Contracts.Enums;

namespace StructuralDashboard.Shared.Entities;

public class FloorProperties
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty; 
    public Model Model { get; set; } = null;   

    public string MaterialId { get; set; } = string.Empty;
    public Material Material { get; set; }
    public StructuralFloorType Type { get; set; }
    public ModelingType ModelingType { get; set; }
    public SlabType SlabType { get; set; }  
    public double Thickness { get; set; }
    public string DeckPropertiesId { get; set; } = string.Empty;        
    public DeckProperties DeckProperties { get; set; } = null;
    public ShearStudProperties ShearStudProperties { get; set; } = null;
    public ShellModifiers ShellModifiers { get; set; } = null;  
}