namespace StructuralDashboard.Shared.Entities;

public class Grid
{
    public string Id { get; set; } = string.Empty; 
    public string Name { get; set; } = null!;
    public Point StartPoint { get; set; } = null!;
    public Point EndPoint { get; set; } = null!;
}
