using System.Collections.Generic;

namespace StructuralDashboard.Shared.Contracts
{
    public class Floor
    {
        public string Id { get; set; }
        public string LevelId { get; set; }
        public string FloorPropertiesId { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();
        public string DiaphragmId { get; set; }
        public string SurfaceLoadId { get; set; }
        public double SpanDirection { get; set; } = 0.0;
        public ShellModifiers ShellModifiers { get; set; } = new ShellModifiers();
    }
}