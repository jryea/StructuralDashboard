namespace StructuralDashboard.Shared.Entities;

public class FrameProperties
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;

    // null! - null forgiving operator,
    // tells the compiler that we are sure this property
    // will be initialized before use,
    // even if the initial value is null
    public Model Model { get; set; } = null!;
    public double? Depth { get; set; }
    public double? Width { get; set; }
    public string SectionName { get; set; } = string.Empty; 
    public SectionType SectionType { get; set; }
}
