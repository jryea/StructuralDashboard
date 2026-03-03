using System.Collections.Generic;

namespace StructuralDashboard.Shared.Contracts
{
    public class Wall
    {
        public string Id { get; set; }
        public List<Point> Points { get; set; }
        public string BaseLevelId { get; set; }
        public string TopLevelId { get; set; }
        public string PropertiesId { get; set; }
        public string PierId { get; set; }
        public string SpandrelId { get; set; }
        public bool IsLateral { get; set; } = false;
    }
}

