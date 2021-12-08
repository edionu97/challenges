using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Classifier.Rules.Impl
{
    public class BlackHoleClassificationRule : ICelestialObjectClassificationRule
    {
        public int RuleOrder => 0;

        public Maybe<CelestialObjectType> Apply(CelestialObject celestialObject)
        {
            return Maybe.None<CelestialObjectType>();
        }
    }
}
