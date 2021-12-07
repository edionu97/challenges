using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Services.Source
{
    public interface IDiscoverySourceService
    {
        /// <summary>
        /// This method should be used for saving a discovery source
        /// </summary>
        /// <param name="name">The name of the discovery source</param>
        /// <param name="establishmentDate">The date on which that source was established</param>
        /// <param name="sourceType">Type of the source</param>
        /// <param name="stateOwner">The owner of the source</param>
        /// <param name="saveChangesImmediately">
        ///     If this boolean is set to true, the values will be immediately saved into the database, otherwise they must be saved manually
        /// </param>
        /// <returns>A task</returns>
        public Task AddDiscoverySourceAsync(
            string name,
            DateTime establishmentDate,
            DiscoverySourceType sourceType,
            string stateOwner,
            bool saveChangesImmediately = true);

        /// <summary>
        /// This method should be used for finding a discovery source
        /// </summary>
        /// <param name="discoverySourceName">The name of the discovery source</param>
        /// <returns>A maybe which can contain either a single element either it could be empty</returns>
        public Task<Maybe<DiscoverySource>>
            FindDiscoverySourceAsync(string discoverySourceName);

        /// <summary>
        /// This method should be used for getting a discovery source by it's partial name
        /// </summary>
        /// <param name="partialName">A string representing partial name</param>
        /// <exception cref="InvalidOperationException">If there are more than on result</exception>
        /// <returns>A maybe which can contain either a single element either it could be empty</returns>
        public Task<Maybe<DiscoverySource>>
            FindDiscoverySourceByPartialName(string partialName);

        /// <summary>
        /// This method should be used for getting all the items async
        /// </summary>
        /// <returns>A list of sources</returns>
        public Task<IEnumerable<DiscoverySource>> GetAllAsync();
    }
}
