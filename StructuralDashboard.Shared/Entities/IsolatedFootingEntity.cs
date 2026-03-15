namespace StructuralDashboard.Shared.Entities;

public class IsolatedFootingEntity
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public ModelEntity Model { get; set; } = null!;
    public double Width { get; set; }
    public double Length { get; set; }
    public double Thickness { get; set; }
    public Point Point { get; set; } = null!;
    public string LevelId { get; set; } = null!;
    public LevelEntity Level { get; set; } = null!;
    public double Orientation { get; set; }
}
