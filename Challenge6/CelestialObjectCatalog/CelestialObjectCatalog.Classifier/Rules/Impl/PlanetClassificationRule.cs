using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using Deveel.Math;

namespace CelestialObjectCatalog.Classifier.Rules.Impl
{
    using Constants = Constants.NumericConstants;

    public class PlanetClassificationRule : ICelestialObjectClassificationRule
    {
        public int RuleOrder => 1;

        public Maybe<CelestialObjectType> Apply(CelestialObject celestialObject)
        {
            //null check
            if (celestialObject == null)
            {
                return Maybe.None<CelestialObjectType>();
            }

            //deconstruct the object
            var (mass, _, _) = celestialObject;

            //if the mass is lower or equal to the upper planet mass limit then there is a planet
            return
                mass.Lt(Constants
                    .UpperPlanetMassLimit)
                ? Maybe.Some(CelestialObjectType.Planet)
                : Maybe.None<CelestialObjectType>();
        }
    }
}
