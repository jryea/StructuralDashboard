namespace StructuralDashboard.Shared.Contracts
{ 
    public class SurfaceLoad
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LayoutTypeId { get; set; }
        public string LiveLoadId { get; set; }
        public double? LiveLoadValue { get; set; }
        public string DeadLoadId { get; set; }
        public double? DeadLoadValue { get; set; }
    }
}