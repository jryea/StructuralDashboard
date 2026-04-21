namespace StructuralDashboard.Web.Models;

public class PlanViewModel
{
    public string LevelId { get; set; } = string.Empty;
    public string LabelMode { get; set; } = "Id";
    public List<PlanMember> Members { get; set; } = new();
    public bool IsVisible { get; set; } = false;
    public double MinX { get; set; }
    public double MinY { get; set; }
    public double MaxX { get; set; }
    public double MaxY { get; set; }
}
