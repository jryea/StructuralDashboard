namespace StructuralDashboard.Web.Models;

public class PlanViewCanvas
{
    public PlanViewModel RevitModel { get; set; }
    public PlanViewModel AnalysisModel { get; set; }
    public bool RevitVisible { get; set; } = true;
    public bool AnalysisVisible { get; set; } = true;
    public AlignmentTransform AnalysisTransform { get; set; } = new();
}
