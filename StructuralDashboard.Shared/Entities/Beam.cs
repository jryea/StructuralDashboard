namespace StructuralDashboard.Shared.Entities;

public class Beam
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public Model Model { get; set; } = null;
    public string LevelId { get; set; } = string.Empty;
    public Level Level { get; set; } = null;
    public Point StartPoint { get; set; } = null;
    public Point EndPoint { get; set; } = null;
    public bool IsJoist { get; set; }   
    public bool IsLateral { get; set; }
    public string FramePropertiesId { get; set; } = string.Empty;
    public FrameProperties FrameProperties { get; set; } = null;    
}
