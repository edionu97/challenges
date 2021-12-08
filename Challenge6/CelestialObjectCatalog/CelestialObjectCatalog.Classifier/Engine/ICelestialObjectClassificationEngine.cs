using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.Classifier.Engine
{
    public interface ICelestialObjectClassificationEngine
    {
        /// <summary>
        /// This method is used for classifying an unclassified object
        /// </summary>
        /// <param name="unclassifiedObject">The object that will be classified</param>
        /// <returns>A object type</returns>
        public CelestialObjectType Classify(CelestialObject unclassifiedObject);
    }
}
