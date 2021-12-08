using Deveel.Math;

namespace CelestialObjectCatalog.Classifier.Constants
{
    public static class NumericConstants
    {
        /// <summary>
        /// Defines the upper planet mass limit
        /// </summary>
        public static readonly BigDecimal UpperPlanetMassLimit = 
            BigMath
                .Multiply(
                    13,
                    BigDecimal.Parse("1.898e27"));

        /// <summary>
        /// Defines the star min temperature
        /// </summary>
        public static readonly BigDecimal StarMinimumSurfaceTemperature = new(2500);

        /// <summary>
        /// Gravitational constant (G)
        /// </summary>
        public static readonly BigDecimal GravitationalConstant = 
            BigDecimal
                .Parse("6.67e-11");

        /// <summary>
        /// Speed of light (C)
        /// </summary>
        public static readonly BigDecimal SpeedOfLight = new(299792458);
    }
}
