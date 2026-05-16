namespace StructuralDashboard.Shared.Contracts
{
    public class Column
    {
        public string Id { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public string BaseLevelId { get; set; }
        public string TopLevelId { get; set; }

        // Web runs west/east at 0 degree orientation 
        public double Orientation { get; set; }
        public string FramePropertiesId { get; set; }
        public bool IsLateral { get; set; } = false;
        public FrameModifiers FrameModifiers { get; set; }
    }
}