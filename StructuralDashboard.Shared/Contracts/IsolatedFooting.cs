namespace StructuralDashboard.Shared.Contracts
{
    public class IsolatedFooting
    {
        public string Id { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public double Thickness { get; set; }
        public Point Point { get; set; }
        public string LevelId { get; set; }
        public string MaterialId { get; set; }
        public double Orientation { get; set; } = 0.0;
    }
}