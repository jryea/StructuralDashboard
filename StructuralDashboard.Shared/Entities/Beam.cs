namespace StructuralDashboard.Shared.Entities;

//The rule of thumb:

//If the FK is required(= null!) → navigation property is also = null!
//If the FK is optional(string?) → navigation property is = null or just Level? Level

public class Beam
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public Model Model { get; set; } = null;
    public string LevelId { get; set; } = null!;
    public Level Level { get; set; } = null!;
    public Point StartPoint { get; set; } = null!;
    public Point EndPoint { get; set; } = null!;
    public bool IsJoist { get; set; }
    public bool IsLateral { get; set; } 
    public string FramePropertiesId { get; set; } = null!;
    public FrameProperties FrameProperties { get; set; } = null!;
}
