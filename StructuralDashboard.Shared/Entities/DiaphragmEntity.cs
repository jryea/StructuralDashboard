namespace StructuralDashboard.Shared.Entities;

public class DiaphragmEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = null!;    
    public DiaphragmType Type { get; set; }
}