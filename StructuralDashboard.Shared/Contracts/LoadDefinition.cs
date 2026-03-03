using StructuralDashboard.Shared.Contracts.Enums;

namespace StructuralDashboard.Shared.Contracts
{   
    public class LoadDefinition
    {
        public string Id { get; set; }

        public LoadType LoadType { get; set; }
        public string Name { get; set; }
        public double SelfWeight { get; set; }
    }
}