namespace StructuralDashboard.Shared.Contracts;

public class LoadPath
{
    public string SelectedMemberId { get; set; }
    public List<string> Upstream { get; set; } = new();
    public List<string> Downstream { get; set; } = new();
}