namespace StructuralDashboard.Shared.Contracts
{
    public class Beam
    {
        public string Id { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public string LevelId { get; set; }
        public string FramePropertiesId { get; set; }
        public bool IsLateral { get; set; }
        public bool IsJoist { get; set; }
        public FrameModifiers FrameModifiers { get; set; }
    }
}