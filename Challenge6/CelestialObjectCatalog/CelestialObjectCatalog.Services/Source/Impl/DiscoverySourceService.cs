using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using CelestialObjectCatalog.Persistence.Models.Enums;
using Microsoft.Extensions.Logging;

namespace CelestialObjectCatalog.Services.Source.Impl
{
    public class DiscoverySourceService : IDiscoverySourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DiscoverySourceService> _logger;


        public DiscoverySourceService(
            IUnitOfWork unitOfWork, 
            ILogger<DiscoverySourceService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AddDiscoverySourceAsync(
            string name,
            DateTime establishmentDate,
            DiscoverySourceType sourceType,
            string stateOwner,
            bool saveChangesImmediately = true)
        {
            //create a new discovery source
            var discoverySource = new DiscoverySource
            {
                Name = name,
                EstablishmentDate = establishmentDate,
                Type = sourceType,
                StateOwner = stateOwner
            };

            //add value into the repository
            await _unitOfWork
                .DiscoverySourceRepo
                .AddAsync(discoverySource);

            //check if changes should be saved
            if (!saveChangesImmediately)
            {
                //log info
                _logger?
                    .LogInformation("Changes will be committed latter");

                return;
            }

            //log info
            _logger?
                .LogInformation("Changes will be committed to database...");

            //commit changes
            await _unitOfWork.CommitAsync();
        }

        public Task<Maybe<DiscoverySource>>
            FindDiscoverySourceAsync(string discoverySourceName) => 
                _unitOfWork
                    .DiscoverySourceRepo
                    .FindSingleAsync(x => x.Name == discoverySourceName);

        public Task<Maybe<DiscoverySource>>
            FindDiscoverySourceByPartialName(string partialName) =>
                _unitOfWork.
                    DiscoverySourceRepo
                    .FindSingleAsync(x => x.Name.Contains(partialName));

        public Task<IEnumerable<DiscoverySource>> GetAllAsync() =>
            _unitOfWork
                .DiscoverySourceRepo
                .GetAllAsync();
    }
}
