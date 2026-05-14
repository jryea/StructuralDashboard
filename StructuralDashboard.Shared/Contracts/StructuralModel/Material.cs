namespace StructuralDashboard.Shared.Contracts
{
    public class Material
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DirectionalSymmetryType? DirectionalSymmetryType { get; set; }
        public MaterialType? MaterialType { get; set; }

        /// <summary>
        /// Weight per unit volume (pcf)
        /// </summary>
        public double? WeightPerUnitVolume { get; set; }

        /// <summary>
        /// Mass per unit volume (p-s^2/ft^4)
        /// </summary>
        public double? MassPerUnitVolume { get; set; }

        /// <summary>
        /// Elastic modulus (psi)
        /// </summary>
        public double? ElasticModulus { get; set; }

        public double? PoissonsRatio { get; set; }

        /// <summary>
        /// Coefficient of thermal expansion (1/°F)
        /// </summary>
        public double? CoefficientOfThermalExpansion { get; set; }

        /// <summary>
        /// Shear modulus (psi)
        /// </summary>
        public double? ShearModulus { get; set; }
        public ConcreteProperties ConcreteProps { get; set; }
        public SteelProperties SteelProps { get; set; }
    }

    public class SteelProperties
    {
        /// <summary>
        /// Minimum yield stress (psi)
        /// </summary>
        public double? Fy { get; set; }


        /// <summary>
        /// Minimum tensile strength (psi)
        /// </summary>
        public double? Fu { get; set; }

        /// <summary>
        /// Expected yield stress (psi)
        /// </summary>
        public double? Fye { get; set; }

        /// <summary>
        /// Expected tensile strength (psi)
        /// </summary>
        public double? Fue { get; set; }

        public string Grade { get; private set; }
    }

    public class ConcreteProperties
    {
        public double? Fc { get; set; }
        public WeightClass? WeightClass { get; set; }
        public double? ShearStrengthReductionFactor { get; set; }
        public string Grade { get; private set; }
    }
}