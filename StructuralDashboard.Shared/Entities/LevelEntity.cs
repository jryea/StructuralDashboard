namespace StructuralDashboard.Shared.Entities;

public class LevelEntity
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public ModelEntity Model { get; set; } = null!;  
    public string Name { get; set; } = null!;
    public double Elevation { get; set; }   
}
