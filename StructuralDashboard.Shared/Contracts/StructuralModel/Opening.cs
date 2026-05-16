using System.Collections.Generic;

namespace StructuralDashboard.Shared.Contracts
{
    public class Opening
    {
        public string Id { get; set; }
        public string LevelId { get; set; }
        public List<Point> Points { get; set; } = new List<Point>();
    }
}