
namespace StructuralDashboard.Shared.Entities;

public class FrameProperties
{
    public string Id { get; set; } = string.Empty;
    public Model Model { get; set; } = null!;
    public string ModelId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string MaterialId { get; set; } = string.Empty;
    public Material Material { get; set; } = null!;
    public ConcreteFrameProperties ConcreteProps{ get; set; } = null;
    public SteelFrameProperties SteelProps { get; set; } = null;
    public WoodFrameProperties WoodProps { get; set; } = null;
    public FrameModifiers FrameModifers { get; set; } = null;
}
