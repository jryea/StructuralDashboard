using System.Collections.Generic;

namespace StructuralDashboard.Shared.Contracts
{
    public class ModelLayoutContainer
    {
        public List<Grid> Grids { get; set; }
        public List<Level> Levels { get; set; }
        public List<FloorType> FloorTypes { get; set; }
    }
}