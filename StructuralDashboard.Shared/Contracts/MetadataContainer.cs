namespace StructuralDashboard.Shared.Contracts
{
    public class MetadataContainer
    {
        public ProjectInfo ProjectInfo { get; set; } = new ProjectInfo();
        public Units Units { get; set; } = new Units();
        public Coordinates Coordinates { get; set; } = new Coordinates();
    }
}