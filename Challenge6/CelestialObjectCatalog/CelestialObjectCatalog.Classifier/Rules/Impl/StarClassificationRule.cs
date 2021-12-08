using System;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Classifier.Rules.Impl
{
    public class StarClassificationRule : ICelestialObjectClassificationRule
    {
        public int RuleOrder => 1;

        public Maybe<CelestialObjectType> Apply(CelestialObject celestialObject)
        {
            return Maybe.None<CelestialObjectType>();
        }
    }
}
