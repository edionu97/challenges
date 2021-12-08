using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Utility.Items;
using Deveel.Math;

namespace CelestialObjectCatalog.Classifier.Rules.Impl
{
    using Constants = Constants.NumericConstants;

    public class BlackHoleClassificationRule : ICelestialObjectClassificationRule
    {
        public int RuleOrder => 0;

        public Maybe<CelestialObjectType> Apply(CelestialObject celestialObject)
        {
            //null check
            if (celestialObject == null)
            {
                return Maybe.None<CelestialObjectType>();
            }

            //destruct the object
            var (mass, diameter, _) = celestialObject;

            //get Rs value
            var schwarzschildRadius =
                BigMath
                    .Divide(
                        2 * Constants.GravitationalConstant * mass, 
                        BigMath.Pow(Constants.SpeedOfLight, 2),
                        MathContext.Decimal128);

            //compute physical radius
            var physicalRadius =
                BigMath
                    .Divide(
                        diameter, 
                        2, 
                        MathContext.Decimal128);

            //is black hole if phr < RS
            return physicalRadius.Lt(schwarzschildRadius)
                ? Maybe.Some(CelestialObjectType.BlackHole)
                : Maybe.None<CelestialObjectType>();
        }
    }
}
