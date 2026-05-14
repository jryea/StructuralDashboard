using System.Collections.Generic;

namespace StructuralDashboard.Shared.Contracts
{
    public class LoadCombination
    {
        public string Id { get; set; }
        public List<string> LoadDefinitionIds { get; set; }  
    }
}