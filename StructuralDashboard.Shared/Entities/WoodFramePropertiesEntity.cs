namespace StructuralDashboard.Shared.Entities;

public class WoodFramePropertiesEntity
{
    public WoodSectionType SectionType { get; set; }
    public string SectionName { get; set; } = null!; 
    public double Depth { get; set; } 
    public double Width { get; set; }   
}
