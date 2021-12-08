using System.Collections.Generic;
using System.Linq;
using CelestialObjectCatalog.Classifier.Rules;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.Classifier.Engine.Impl
{
    public class CelestialObjectClassificationEngine : ICelestialObjectClassificationEngine
    {
        private readonly IEnumerable<ICelestialObjectClassificationRule> _classificationRules;

        public CelestialObjectClassificationEngine(
            IEnumerable<ICelestialObjectClassificationRule> classificationRules)
        {
            //classification rule
            _classificationRules = 
                classificationRules
                    .OrderBy(x => x.RuleOrder);
        }

        public CelestialObjectType Classify(CelestialObject unclassifiedObject)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var celestialObjectClassificationRule in _classificationRules)
            {
                //get the optional status
                var optionalStatus = 
                    celestialObjectClassificationRule.Apply(unclassifiedObject);

                //rule cannot be applied
                if (!optionalStatus.Any())
                {
                    continue;
                }

                //return the object
                return optionalStatus.Single();
            }

            //if object cannot be classified use unknown type
            return CelestialObjectType.Unknown;
        }
    }
}
