using System.Collections.Generic;

namespace StructuralDashboard.Shared.Contracts
{
    public class PropertiesContainer
    {
        public List<Material> Materials { get; set; }
        public List<WallProperties> WallProperties { get; set; }
        public List<FloorProperties> FloorProperties { get; set; }
        public List<Diaphragm> Diaphragms { get; set; }
        public List<FrameProperties> FrameProperties { get; set; }
    }
}