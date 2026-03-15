namespace StructuralDashboard.Shared.Entities;

public class Opening
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;     
    public Model Model { get; set; } = null!;
    public string LevelId { get; set; } = null!;
    public Level Level { get; set; } = null!;
    public List<Point> Points { get; set; } = new List<Point>();    
}
