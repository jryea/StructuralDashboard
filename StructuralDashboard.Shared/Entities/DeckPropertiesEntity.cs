namespace StructuralDashboard.Shared.Entities;

public class DeckPropertiesEntity
{
    public string DeckType { get; set; } = null!;
    public string? MaterialId { get; set; }
    public double RibDepth { get; set; }
    public double RibWidthTop { get; set; }
    public double RibWidthBottom { get; set; }
    public double RibSpacing { get; set; }
    public double DeckShearThickness { get; set; }
    public double DeckUnitWeight { get; set; }
}