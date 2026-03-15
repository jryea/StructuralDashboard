namespace StructuralDashboard.Shared.Entities;

public class FloorEntity
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public ModelEntity Model { get; set; } = null!;
    public string LevelId { get; set; } = null!;
    public LevelEntity Level { get; set; } = null!;
    public string FloorPropertiesId { get; set; } = null!;
    public FloorPropertiesEntity FloorProperties { get; set; } = null!;
    public List<Point> Points { get; set; } = new List<Point>();
    public string? DiaphragmId { get; set; } = null;
    public DiaphragmEntity? Diaphragm { get; set; } = null;
    public string? SurfaceLoadId { get; set; } = null;   
    public SurfaceLoadEntity? SurfaceLoad { get; set; } = null;    
    public double SpanDirection { get; set; }
    public ShellModifiersEntity? ShellModifiers { get; set; } = null;
}
