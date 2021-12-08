using CelestialObjectCatalog.Classifier.Engine;
using CelestialObjectCatalog.Classifier.Engine.Impl;
using CelestialObjectCatalog.Classifier.Rules;
using CelestialObjectCatalog.Classifier.Rules.Impl;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using Deveel.Math;
using System.Collections.Generic;
using Xunit;

namespace CelestialObjectCatalog.Classifier.UnitTests
{
    public class RulesUnitTests
    {
        private readonly ICelestialObjectClassificationEngine _sut;

        public RulesUnitTests()
        {
            //define rules
            var rules = new List<ICelestialObjectClassificationRule>
            {
                new StarClassificationRule(),
                new BlackHoleClassificationRule(),
                new PlanetClassificationRule()
            };

            //declare sut
            _sut = new CelestialObjectClassificationEngine(rules);
        }

        [Theory]
        [Trait("Category", "RulesUnitTests")]
        [InlineData("5.97237e24", "12756200", "5800", CelestialObjectType.Planet)]
        [InlineData("4.2e40", "4280000", "2000", CelestialObjectType.BlackHole)]
        [InlineData("3.65e29", "184502000", "4800", CelestialObjectType.Star)]
        [InlineData("2.91e28", "55000000", "1600", CelestialObjectType.Unknown)]
        [InlineData("1.3e40", "11600000", "2000", CelestialObjectType.BlackHole)]
        [InlineData("3.17237e25", "21557978", "200", CelestialObjectType.Planet)]
        [InlineData("1.87532418e27", "14010000", "300", CelestialObjectType.Planet)]
        [InlineData("3.4164e28", "9.94e8", "3500", CelestialObjectType.Star)]
        [InlineData("1.2e30", "90000000", "1000", CelestialObjectType.Unknown)]
        [InlineData("7.634e31", "1.6710622e10", "5800", CelestialObjectType.Star)]
        public void TestClassification(
            string mass,
            string equatorialDiameter,
            string surfaceTemperature,
            CelestialObjectType type)
        {
            //classify the object
            var result = _sut
                .Classify(new CelestialObject
                {
                    Mass = BigDecimal.Parse(mass),
                    EquatorialDiameter = BigDecimal.Parse(equatorialDiameter),
                    SurfaceTemperature = BigDecimal.Parse(surfaceTemperature)
                });

            //assert results
            Assert.Equal(type, result);
        }
    }
}
