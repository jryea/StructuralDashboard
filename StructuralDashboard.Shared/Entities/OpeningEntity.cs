namespace StructuralDashboard.Shared.Entities;

public class OpeningEntity
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;     
    public StructuralModelEntity Model { get; set; } = null!;
    public string LevelId { get; set; } = null!;
    public LevelEntity Level { get; set; } = null!;
    public List<Point> Points { get; set; } = new List<Point>();    
}
