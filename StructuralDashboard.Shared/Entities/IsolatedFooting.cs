using System;
using System.Collections.Generic;
using System.Text;

namespace StructuralDashboard.Shared.Entities;

public class IsolatedFooting
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public Model Model { get; set; } = null!;
    public double Width { get; set; }
    public double Length { get; set; }
    public double Thickness { get; set; }
    public Point Point { get; set; } = null!;
    public string LevelId { get; set; } = null!;
    public Level Level { get; set; } = null!;
    public double Orientation { get; set; }
}
