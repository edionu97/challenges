using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Classifier.Rules
{
    public interface ICelestialObjectClassificationRule
    {
        /// <summary>
        /// Defines the order of the rule
        /// </summary>
        public int RuleOrder { get; }
        
        /// <summary>
        /// This method can be used for computing the celestial object type
        /// </summary>
        /// <param name="celestialObject">The celestial object containing all the required info</param>
        /// <returns>An empty maybe if the rule cannot be applied or an maybe containing the celestial type</returns>
        public Maybe<CelestialObjectType> Apply(CelestialObject celestialObject);
    }
}
