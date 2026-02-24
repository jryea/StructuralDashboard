namespace StructuralDashboard.Shared.Entities;

public class Model
{
    public string Id { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string ProjectNumber { get; set; } = string.Empty;
    public Project Project { get; set; } = null!;
    public string SavedBy { get; set; } = string.Empty;
    public DateTime SavedAtUtc { get; set; }
    public SourceApplication SourceApplication { get; set; }
}
