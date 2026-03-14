using StructuralDashboard.Shared.Contracts.Enums;

namespace StructuralDashboard.Shared.Entities;

public class SteelFrameProperties
{
    public SteelSectionType SectionType { get; set; }
    public string SectionName { get; set; } = string.Empty; 
}
