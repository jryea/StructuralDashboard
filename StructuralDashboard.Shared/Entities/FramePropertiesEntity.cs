
namespace StructuralDashboard.Shared.Entities;

public class FramePropertiesEntity
{
    public string Id { get; set; } = string.Empty;
    public ModelEntity Model { get; set; } = null!;
    public string ModelId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string MaterialId { get; set; } = null!;
    public MaterialEntity Material { get; set; } = null!;

    public ConcreteFramePropertiesEntity? ConcreteProps { get; set; }
    public SteelFramePropertiesEntity? SteelProps { get; set; }
    public WoodFramePropertiesEntity? WoodProps { get; set; }
    public FrameModifiersEntity? FrameModifiers { get; set; }
}
