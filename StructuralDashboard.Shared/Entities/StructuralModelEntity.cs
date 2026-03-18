namespace StructuralDashboard.Shared.Entities;

public class StructuralModelEntity
{
    public string Id { get; set; } = string.Empty;
    public string ModelName { get; set; } = null!;
    public string ProjectNumber { get; set; } = null!;
    public ProjectEntity Project { get; set; } = null!;
    public string SavedBy { get; set; } = null!;
    public DateTime SavedAtUtc { get; set; }
    public SourceApplication SourceApplication { get; set; }
}
