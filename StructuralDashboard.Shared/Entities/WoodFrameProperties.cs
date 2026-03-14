using StructuralDashboard.Shared.Contracts.Enums;

namespace StructuralDashboard.Shared.Entities;

public class WoodFrameProperties
{
    public WoodSectionType SectionType { get; set; }
    public string SectionName { get; set; } = string.Empty; 
    public double Depth { get; set; } 
    public double Width { get; set; }   
}
