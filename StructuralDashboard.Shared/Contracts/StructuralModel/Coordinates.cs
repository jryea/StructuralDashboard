namespace StructuralDashboard.Shared.Contracts
{
    public class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Rotation { get; set; } // Rotation angle from project north

        // Project Base Point
        public Point ProjectBasePoint { get; set; }

        // Survey Point
        public Point SurveyPoint { get; set; }

        // Custom User Selected Coordination Point
        public Point CoordinationPoint { get; set; }
    }
}