using System;
using StructuralDashboard.Shared.Contracts.Enums;
using System.Collections.Generic;
using System.Text;

namespace StructuralDashboard.Shared.Entities;

public class ConcreteFrameProperties
{
    public ConcreteSectionType SectionType { get; set; }
    public string SectionName { get; set; } = null!;
    public double Depth { get; set; } 
    public double Width { get; set; }
}
