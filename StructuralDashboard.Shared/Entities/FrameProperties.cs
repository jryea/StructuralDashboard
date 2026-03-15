
namespace StructuralDashboard.Shared.Entities;

public class FrameProperties
{
    public string Id { get; set; } = string.Empty;
    public Model Model { get; set; } = null!;
    public string ModelId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string MaterialId { get; set; } = null!;
    public Material Material { get; set; } = null!;

    public ConcreteFrameProperties? ConcreteProps { get; set; }
    public SteelFrameProperties? SteelProps { get; set; }
    public WoodFrameProperties? WoodProps { get; set; }
    public FrameModifiers? FrameModifers { get; set; }
}
