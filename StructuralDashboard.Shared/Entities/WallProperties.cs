namespace StructuralDashboard.Shared.Entities;

public class WallProperties
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public Model Model { get; set; } = null;
    public string Name { get; set; } = string.Empty;    
    public string MaterialId { get; set; } = string.Empty;  
    public Material Material { get; set; } = null;
    public double Thickness { get; set; }
}