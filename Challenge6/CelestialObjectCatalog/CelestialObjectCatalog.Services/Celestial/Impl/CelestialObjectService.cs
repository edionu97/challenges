using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Exceptions;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Persistence.Repository;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using CelestialObjectCatalog.Services.Source;
using CelestialObjectCatalog.Utility.Items;
using Microsoft.EntityFrameworkCore;

namespace CelestialObjectCatalog.Services.Celestial.Impl
{
    public partial class CelestialObjectService : ICelestialObjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDiscoverySourceService _discoverySourceService;

        private readonly IRepository<CelestialObject, Guid> _celestialObjectRepository;

        public CelestialObjectService(
            IUnitOfWork unitOfWork, 
            IDiscoverySourceService discoverySourceService,
            IRepository<CelestialObject, Guid> celestialObjectRepository)
        {
            _unitOfWork = unitOfWork;
            _discoverySourceService = discoverySourceService;
            _celestialObjectRepository = celestialObjectRepository;
        }

        public async Task AddAsync(
            string objectName, 
            double objectMass, 
            double objectDiameter, 
            double objectTemperature,
            string discoverySourceName,
            DateTime? objectDiscoveryDate = null,
            bool saveChangesImmediately = true)
        {
            //get discovery source optional
            var discoverySource = 
                await GetObjectDiscoverySourceByNameAsync(discoverySourceName);

            //try get celestial object
            var celestialObjectOptional = 
                await FindCelestialObjectAsync(objectName);

            //set discovery date
            var discoveryDate = objectDiscoveryDate ?? DateTime.UtcNow;

            //create a function to be called with different actions (Add or update)
            Func<Task> addOrUpdate = 
                celestialObjectOptional.Any()
                    //if object already exists into database just add an extra discovery source to it
                    ? async () => await AddExtraDiscoverySourceAsync(
                            discoverySource, 
                            celestialObjectOptional.Single(),
                            discoveryDate)
                    //otherwise create the object
                    : async () => await CreateCelestialObjectAsync(
                            objectName,
                            objectMass,
                            objectDiameter,
                            objectTemperature,
                            discoverySource,
                            discoveryDate);

            //call the method
            await addOrUpdate();

            //if we should not save changes immediately return
            if (!saveChangesImmediately)
            {
                return;;
            }

            //commit
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CelestialObject>> GetAllAsync()
        {
            return await _celestialObjectRepository
                .GetAllAsQueryable()
                .ToListAsync();
        }

        public Task<IEnumerable<CelestialObject>> GetAllObjectByTypeAsync(CelestialObjectType objectType)
        {
            throw new System.NotImplementedException();
        }

        public Task<Maybe<CelestialObject>> 
            FindCelestialObjectAsync(string objectName) =>
                _celestialObjectRepository
                    .FindSingleAsync(
                        x=> x.Name == objectName,
                        x => x.CelestialObjectDiscoveries);

        public Task<IEnumerable<CelestialObject>> GetObjectsDiscoveredByCountryAsync(string countryName)
        {
            throw new System.NotImplementedException();
        }
    }
}
