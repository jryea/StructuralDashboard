namespace StructuralDashboard.Shared.Contracts
{
    public class FrameModifiers
    {
        // Axial properties
        public double? Area { get; set; }            // Cross-sectional area modifier

        // Shear properties  
        public double? A22 { get; set; }             // Shear area modifier in local 2-direction
        public double? A33 { get; set; }             // Shear area modifier in local 3-direction
        // Bending properties
        public double? I22 { get; set; }             // Moment of inertia in local 2-direction (major axis)
        public double? I33 { get; set; }             // Moment of inertia in local 3-direction (minor axis)
        // Torsional properties
        public double? Torsion { get; set; }         // Torsional constant modifier

        // Mass properties
        public double? Mass { get; set; }            // Mass modifier
        public double? Weight { get; set; }          // Weight modifier
    }

    /// <summary>
    /// Shell element stiffness modifiers for floors, walls, and slabs
    /// </summary>
    public class ShellModifiers
    {
        // Membrane (in-plane) stiffness modifiers
        public double? F11 { get; set; }                 // In-plane stiffness in local 1-direction
        public double? F22 { get; set; }                 // In-plane stiffness in local 2-direction
        public double? F12 { get; set; }                 // In-plane shear stiffness

        // Bending (out-of-plane) stiffness modifiers
        public double? M11 { get; set; }                 // Bending stiffness in local 1-direction
        public double? M22 { get; set; }                 // Bending stiffness in local 2-direction
        public double? M12 { get; set; }                 // Twisting stiffness

        // Transverse shear stiffness modifiers
        public double? V13 { get; set; }                // Out-of-plane shear in 1-3 plane
        public double? V23 { get; set; }                // Out-of-plane shear in 2-3 plane

        // Mass properties
        public double? Mass { get; set; }               // Mass modifier
        public double? Weight { get; set; }             // Weight modifier
    }
}