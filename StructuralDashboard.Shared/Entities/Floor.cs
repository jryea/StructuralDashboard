namespace StructuralDashboard.Shared.Entities;

public class Floor
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public Model Model { get; set; } = null;
    public string LevelId { get; set; } = string.Empty;
    public Level Level { get; set; } = null;
    public string FloorPropertiesId { get; set; } = string.Empty;
    public FloorProperties FloorProperties { get; set; } = null;
    public List<Point> Points { get; set; } = new List<Point>();
    public string DiaphragmId { get; set; } = string.Empty;
    public Diaphragm Diaphragm { get; set; } = null;
    public string SurfaceLoadId { get; set; } = string.Empty;   
    public SurfaceLoad SurfaceLoad { get; set; } = null;    
    public double SpanDirection { get; set; } = 0.0;
    public ShellModifiers ShellModifiers { get; set; } = null;
}
