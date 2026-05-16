namespace StructuralDashboard.Shared.Contracts
{
    public class FloorProperties
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MaterialId { get; set; }
        public StructuralFloorType Type { get; set; }
        public ModelingType ModelingType { get; set; } = ModelingType.Membrane;
        public SlabType SlabType { get; set; } = SlabType.Slab;
        public double Thickness { get; set; }
        public DeckProperties DeckProperties { get; set; }
        public ShearStudProperties ShearStudProperties { get; set; }
        public ShellModifiers ShellModifiers { get; set; } = new ShellModifiers();
    }

    public class ShearStudProperties
    {
        public double ShearStudDiameter { get; set; }
        public double ShearStudHeight { get; set; }
        public double ShearStudTensileStrength { get; set; }

        public ShearStudProperties()
        {
            ShearStudDiameter = 0.75; // Default diameter in inches
            ShearStudHeight = 6.0; // Default height in inches      
            ShearStudTensileStrength = 65000; // Default tensile strength in psi  
        }
    }

    public class DeckProperties
    {
        public string DeckType { get; set; } = "VULCRAFT 2VL";

        // Deck Material ID for ETABS
        public string MaterialID { get; set; }

        public double RibDepth { get; set; }

        public double RibWidthTop { get; set; }
        public double RibWidthBottom { get; set; }
        public double RibSpacing { get; set; }
        public double DeckShearThickness { get; set; }
        public double DeckUnitWeight { get; set; }
    }
}