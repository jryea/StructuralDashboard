namespace StructuralDashboard.Web.Models;

public class PlanViewModel
{
    public string LevelId { get; set; } = string.Empty;
    public string LabelMode { get; set; } = "Id";
    public List<PlanMember> Members { get; set; } = new();
    public double MinX { get; set; }
    public double MinY { get; set; }
    public double MaxX { get; set; }
    public double MaxY { get; set; }
}

public class PlanMember
{
    public string Id { get; set; }
    public string Type { get; set; }
    public double X1 { get; set; }
    public double Y1 { get; set; }
    public double X2 { get; set; }
    public double Y2 { get; set; }
    public double Orientation { get; set; }
    public string Label { get; set; }
    public double TagX { get; set; }
    public double TagY { get; set; }
    public double TagRotation { get; set; }
}
