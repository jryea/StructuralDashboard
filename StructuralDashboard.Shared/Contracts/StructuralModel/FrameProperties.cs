using System.Collections.Generic;

namespace StructuralDashboard.Shared.Contracts
{

    public class FrameProperties
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MaterialId { get; set; }
        public MaterialType MaterialType { get; set; }
        public ConcreteFrameProperties ConcreteProps { get; set; }
        public SteelFrameProperties SteelProps { get; set; }
        public WoodFrameProperties WoodProps { get; set; }  
        public FrameModifiers FrameModifiers { get; set; } = new FrameModifiers();
    }

    public class SteelFrameProperties
    {
        public SteelSectionType SectionType { get; set; }
        public string SectionName { get; set; }
    }

    public class ConcreteFrameProperties
    {
        public ConcreteSectionType SectionType { get; set; }

        public string SectionName { get; set; }

        public double Depth { get; set; } = 12.0;

        public double Width { get; set; } = 12.0;

        public Dictionary<string, string> Dimensions = new Dictionary<string, string>();
    }

    public class WoodFrameProperties
    {
        public WoodSectionType SectionType { get; set; }

        public string SectionName { get; set; }

        public double Depth { get; set; } = 3.5;

        public double Width { get; set; } = 3.5;
    }

}