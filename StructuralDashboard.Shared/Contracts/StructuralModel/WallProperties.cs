namespace StructuralDashboard.Shared.Contracts
{
    public class WallProperties
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MaterialId { get; set; }
        public MaterialType MaterialType { get; set; }
        public double? Thickness { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public double? UnitWeightForSelfWeight { get; set; }
        public ShellModifiers ETABSModifiers { get; set; }
    }

    public class Reinforcement
    {
        public double? FyDistributed { get; set; }
        public double? FuDistributed { get; set; }
        public double? FyTiesLinks { get; set; }
    }
}