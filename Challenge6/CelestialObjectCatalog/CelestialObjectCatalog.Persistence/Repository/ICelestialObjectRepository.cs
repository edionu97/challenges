using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.Persistence.Repository
{
    public interface ICelestialObjectRepository : IRepository<CelestialObject, Guid>
    {
        /// <summary>
        /// This method can be used for getting all objects of a given type
        /// </summary>
        /// <param name="objectType">The object type</param>
        /// <returns>A list of all object that have a particular type</returns>
        public Task<IEnumerable<CelestialObject>> 
            GetAllObjectsOfTypeAsync(CelestialObjectType objectType);

        /// <summary>
        /// This method should be used for getting an object that has a particular name
        /// </summary>
        /// <param name="objectName">The name of the object</param>
        /// <returns>A maybe containing the element, if it exist or an empty maybe otherwise</returns>
        public Task<Maybe<CelestialObject>>
            GetObjectByNameAsync(string objectName);

        /// <summary>
        /// This method should be used for getting all the objects that were discovered by a country
        /// </summary>
        /// <param name="countryName">The name of the country</param>
        /// <returns>A list of objects</returns>
        public Task<IEnumerable<CelestialObject>>
            GetAllObjectDiscoveredByAsync(string countryName);
    }
}
