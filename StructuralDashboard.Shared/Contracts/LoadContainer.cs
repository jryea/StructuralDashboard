using System.Collections.Generic;

namespace StructuralDashboard.Shared.Contracts
{
    public class LoadContainer
    {
        public List<LoadDefinition> LoadDefinitions { get; set; } = new List<LoadDefinition>();
        public List<SurfaceLoad> SurfaceLoads { get; set; } = new List<SurfaceLoad>();
        public List<LoadCombination> LoadCombinations { get; set; } = new List<LoadCombination>();
    }
}