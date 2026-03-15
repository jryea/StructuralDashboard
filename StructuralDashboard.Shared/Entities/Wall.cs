using System;
using System.Collections.Generic;
using System.Text;

namespace StructuralDashboard.Shared.Entities;

public class Wall
{
    public string Id { get; set; } = string.Empty;
    public string ModelId { get; set; } = null!;
    public Point StartPoint { get; set; } = null!;
    public Point EndPoint { get; set; } = null!;
    public Model Model { get; set; } = null!;
    public string BaseLevelId { get; set; } = null!;
    public Level BaseLevel { get; set; } = null!;
    public string TopLevelId { get; set; } = null!;
    public Level TopLevel { get; set; } = null!;
    public string PropertiesId { get; set; } = null!;
    public WallProperties Properties { get; set; } = null!;
    public bool IsLateral { get; set; } 
}
