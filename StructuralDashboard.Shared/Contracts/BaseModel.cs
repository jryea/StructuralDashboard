namespace StructuralDashboard.Shared.Contracts
{
    public class BaseModel
    {
        public string Id { get; set; }
        public ElementContainer Elements { get; set; } = new ElementContainer();
        public LoadContainer Loads { get; set; } = new LoadContainer();
        public PropertiesContainer Properties { get; set; } = new PropertiesContainer();
        public ModelLayoutContainer ModelLayout { get; set; } = new ModelLayoutContainer();
        public MetadataContainer Metadata { get; set; } = new MetadataContainer();
    }
}