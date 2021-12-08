using Deveel.Math;
using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.Classifier.Rules.Impl
{
    using Constants = Constants.NumericConstants;

    public class StarClassificationRule : ICelestialObjectClassificationRule
    {
        public int RuleOrder => 1;

        public Maybe<CelestialObjectType>  Apply(CelestialObject celestialObject)
        {
            //treat null case
            if (celestialObject == null)
            {
                return Maybe.None<CelestialObjectType>();
            }

            //get mass and temp
            var (mass, _, temp) = celestialObject;

            //the object is star only if the mass is >= 13 * jupiter's mass and surface tmp is >= 2500
            return
                mass.Gte(Constants
                    .UpperPlanetMassLimit)
                && temp.Gte(Constants
                    .StarMinimumSurfaceTemperature)
                    ? Maybe.Some(CelestialObjectType.Star)
                    : Maybe.None<CelestialObjectType>();
        }
    }
}
