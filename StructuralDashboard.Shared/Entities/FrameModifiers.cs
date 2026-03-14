using System;
using System.Collections.Generic;
using System.Text;

namespace StructuralDashboard.Shared.Entities;

public class FrameModifiers
{
    public string Id { get; set; }
    public double Area { get; set; }
    public double A22 { get; set; } 
    public double A33 { get; set; }
    public double I22 { get; set; }
    public double I33 { get; set; }
    public double Torsion { get; set; }
    public double Mass { get; set; }
    public double Weight { get; set; }
}
