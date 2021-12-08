using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.Classifier.Rules.Impl
{
    public class PlanetClassificationRule : ICelestialObjectClassificationRule
    {
        public int RuleOrder => 1;

        public Maybe<CelestialObjectType> Apply(CelestialObject celestialObject)
        {
            return Maybe.None<CelestialObjectType>();
        }
    }
}
