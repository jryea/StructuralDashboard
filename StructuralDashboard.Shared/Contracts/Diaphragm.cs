using StructuralDashboard.Shared.Contracts.Enums;

namespace StructuralDashboard.Shared.Contracts
{
    public class Diaphragm
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public DiaphragmType Type { get; set; }
    }
}