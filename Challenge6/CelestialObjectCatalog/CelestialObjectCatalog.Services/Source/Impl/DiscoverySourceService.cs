using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Repository;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.Services.Source.Impl
{
    public class DiscoverySourceService : IDiscoverySourceService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IRepository<DiscoverySource, Guid> _discoverySourceRepository;

        public DiscoverySourceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _discoverySourceRepository = unitOfWork.DiscoverySourceRepo;
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
            await _discoverySourceRepository
                .AddAsync(discoverySource);

            //check if changes should be saved
            if (!saveChangesImmediately)
            {
                return;
            }

            //commit changes
            await _unitOfWork.CommitAsync();
        }

        public Task<Maybe<DiscoverySource>>
            FindDiscoverySourceAsync(string discoverySourceName) =>
                _discoverySourceRepository
                    .FindSingleAsync(
                        x => x.Name == discoverySourceName,
                        x => x.CelestialObjectDiscoveries);

        public Task<Maybe<DiscoverySource>>
            FindDiscoverySourceByPartialName(string partialName) =>
                _discoverySourceRepository
                    .FindSingleAsync(
                        x => x.Name.Contains(partialName),
                        x => x.CelestialObjectDiscoveries);

        public Task<IEnumerable<DiscoverySource>> GetAllAsync() => _discoverySourceRepository.GetAllAsync();
    }
}
