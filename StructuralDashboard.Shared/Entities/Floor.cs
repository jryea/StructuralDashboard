namespace StructuralDashboard.Shared.Entities;

public class Floor
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public Model Model { get; set; } = null!;
    public string LevelId { get; set; } = null!;
    public Level Level { get; set; } = null!;
    public string FloorPropertiesId { get; set; } = null!;
    public FloorProperties FloorProperties { get; set; } = null!;
    public List<Point> Points { get; set; } = new List<Point>();
    public string? DiaphragmId { get; set; } = null;
    public Diaphragm? Diaphragm { get; set; } = null;
    public string? SurfaceLoadId { get; set; } = null;   
    public SurfaceLoad? SurfaceLoad { get; set; } = null;    
    public double SpanDirection { get; set; }
    public ShellModifiers? ShellModifiers { get; set; } = null;
}
