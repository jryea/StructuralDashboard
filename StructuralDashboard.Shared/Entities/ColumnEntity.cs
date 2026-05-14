namespace StructuralDashboard.Shared.Entities;

public class ColumnEntity
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public StructuralModelEntity Model { get; set; } = null!;
    public Point StartPoint { get; set; } = null!;
    public Point EndPoint { get; set; } = null!;
    public string BaseLevelId { get; set; } = null!;
    public LevelEntity BaseLevel { get; set; } = null!;
    public string TopLevelId { get; set; } = null!;
    public LevelEntity TopLevel { get; set; } = null!;
    public double Orientation { get; set; }
    public bool IsLateral { get; set; }   
    public string? FramePropertiesId { get; set; } = null!;
    public FramePropertiesEntity? FrameProperties { get; set; } = null!;
    public FrameModifiersEntity? FrameModifiers { get; set; }
}
