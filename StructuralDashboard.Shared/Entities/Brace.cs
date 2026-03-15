namespace StructuralDashboard.Shared.Entities;
 
public class Brace
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public Model Model { get; set; } = null!;
    public string BaseLevelId { get; set; } = null!;
    public Level BaseLevel { get; set; } = null!;
    public string TopLevelId { get; set; } = null!;
    public Level TopLevel { get; set; } = null!;
    public Point StartPoint { get; set; } = null!;
    public Point EndPoint { get; set; } = null!;
    public string FramePropertiesId { get; set; } = null!;   
    public FrameProperties FrameProperties { get; set; } = null!;
    public FrameModifiers? FrameModifiers { get; set; } = null;
}
