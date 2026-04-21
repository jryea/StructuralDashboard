namespace StructuralDashboard.Web.Models;

public class BeamMatch
{
    public string RevitId { get; set; }
    public string RevitSize { get; set; }
    public string AnalysisId { get; set; }
    public string AnalysisSize { get; set; }
    public string Status { get; set; }  // "Match", "Size", "R Only", "A Only"
}