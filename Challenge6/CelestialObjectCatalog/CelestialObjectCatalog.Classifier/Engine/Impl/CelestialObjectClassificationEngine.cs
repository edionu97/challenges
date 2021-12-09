using System.Collections.Generic;
using System.Linq;
using CelestialObjectCatalog.Classifier.Rules;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using Microsoft.Extensions.Logging;

namespace CelestialObjectCatalog.Classifier.Engine.Impl
{
    public class CelestialObjectClassificationEngine : ICelestialObjectClassificationEngine
    {
        private readonly IEnumerable<ICelestialObjectClassificationRule> _classificationRules;
        private readonly ILogger<CelestialObjectClassificationEngine> _logger;

        public CelestialObjectClassificationEngine(
            IEnumerable<ICelestialObjectClassificationRule> classificationRules, 
            ILogger<CelestialObjectClassificationEngine> logger)
        {
            _logger = logger;

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
                //lof info
                _logger?
                    .LogInformation(
                        $"Applying rule: {celestialObjectClassificationRule.GetType().Name}");

                //get the optional status
                var optionalStatus = 
                    celestialObjectClassificationRule.Apply(unclassifiedObject);

                //rule cannot be applied
                if (!optionalStatus.Any())
                {
                    //log message
                    _logger?
                        .LogInformation(
                            $"Celestial object does not satisfy {celestialObjectClassificationRule}...");
                    continue;
                }

                //get object status
                var celestialObjectType = 
                    optionalStatus.Single();

                //log message
                _logger?
                    .LogInformation(
                        $"Celestial object matches {celestialObjectClassificationRule}...," +
                        $" it's type is: '{celestialObjectType}'");

                //return the object
                return celestialObjectType;
            }

            //if object cannot be classified use unknown type
            return CelestialObjectType.Unknown;
        }
    }
}
