using System;
using CelestialObjectCatalog.Persistence.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Services.Celestial
{
    public interface ICelestialObjectService
    {
        /// <summary>
        /// This function it is responsible for adding a new celestial object into database 
        /// </summary>
        /// <param name="objectName">The name of the object</param>
        /// <param name="objectMass">The object's mass</param>
        /// <param name="objectDiameter">The object's diameter</param>
        /// <param name="objectTemperature">The object's temperature</param>
        /// <param name="objectDiscoveryDate">Date on which object was discovered</param>
        /// <param name="discoverySourceName">The name of the source which discovered the object</param>
        /// <param name="saveChangesImmediately">If true changes will be saved immediately</param>
        /// <returns>A task</returns>
        public Task AddAsync(
            string objectName,
            double objectMass,
            double objectDiameter,
            double objectTemperature,
            string discoverySourceName,
            DateTime? objectDiscoveryDate = null,
            bool saveChangesImmediately = true);

        /// <summary>
        /// This method should be used for getting all the items from the celestial database
        /// </summary>
        /// <returns>A list of celestial objects</returns>
        public Task<IEnumerable<CelestialObject>> GetAllAsync();

        /// <summary>
        /// This method can be used for getting all objects by a specific type
        /// </summary>
        /// <param name="objectType">Type of the object</param>
        /// <returns>A list of celestial object that have a particular type</returns>
        public Task<IEnumerable<CelestialObject>>
            GetAllObjectByTypeAsync(CelestialObjectType objectType);

        /// <summary>
        /// This method can be used for getting the object that has a particular name
        /// </summary>
        /// <param name="objectName">The name of the object</param>
        /// <returns>A maybe which can or cannot contain an element</returns>
        public Task<Maybe<CelestialObject>> 
            FindCelestialObjectAsync(string objectName);

        /// <summary>
        /// Get a list containing all the object discovered by a particular country
        /// </summary>
        /// <param name="countryName">The name of the country</param>
        /// <returns>A list of items</returns>
        public Task<IEnumerable<CelestialObject>> 
            GetObjectsDiscoveredByCountryAsync(string countryName);
    }
}
