namespace StructuralDashboard.Shared.Entities;

public class Level
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public Model Model { get; set; } = null!;  
    public string Name { get; set; } = string.Empty;
    public double Elevation { get; set; }   
}
