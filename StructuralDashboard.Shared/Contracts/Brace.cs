namespace StructuralDashboard.Shared.Contracts
{
    /// <summary>
    /// BRACE CONVENTION: StartPoint is at TOP elevation (TopLevelId). EndPoint is at BOTTOM elevation (BaseLevelId).
    /// </summary>
    public class Brace
    {
        public string Id { get; set; }
        public string MaterialId { get; set; }
        public string FramePropertiesId { get; set; }
        public string BaseLevelId { get; set; }
        public string TopLevelId { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public FrameModifiers FrameModifiers { get; set; }
    }
}