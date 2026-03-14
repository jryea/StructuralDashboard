namespace StructuralDashboard.Shared.Entities;

public class Column
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public Model Model { get; set; } = null;
    public Point StartPoint { get; set; } = null;
    public Point EndPoint { get; set; } = null;
    public string BaseLevelId { get; set; } = string.Empty;
    public Level BaseLevel { get; set; } = null;
    public string TopLevelId { get; set; } = string.Empty;
    public Level TopLevel { get; set; } = null;
    public double Orientation { get; set; } = 0.0;
    public bool IsLateral { get; set; }   
}
