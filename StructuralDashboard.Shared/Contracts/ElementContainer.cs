using System.Collections.Generic;

namespace StructuralDashboard.Shared.Contracts
{
    public class ElementContainer
    {
        public List<Floor> Floors { get; set; } = new List<Floor>();
        public List<Wall> Walls { get; set; } = new List<Wall>();
        public List<Beam> Beams { get; set; } = new List<Beam>();
        public List<Brace> Braces { get; set; } = new List<Brace>();
        public List<Column> Columns { get; set; } = new List<Column>();
        public List<IsolatedFooting> IsolatedFootings { get; set; } = new List<IsolatedFooting>();  
        public List<Opening> Openings { get; set; } = new List<Opening>();
    }
}