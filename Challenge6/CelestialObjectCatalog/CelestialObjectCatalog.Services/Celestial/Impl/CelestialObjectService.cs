using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Services.Source;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using CelestialObjectCatalog.Persistence.Models.Enums;
using Deveel.Math;

namespace CelestialObjectCatalog.Services.Celestial.Impl
{
    public partial class CelestialObjectService : ICelestialObjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDiscoverySourceService _discoverySourceService;

        public CelestialObjectService(
            IUnitOfWork unitOfWork,
            IDiscoverySourceService discoverySourceService)
        {
            _unitOfWork = unitOfWork;
            _discoverySourceService = discoverySourceService;
        }

        public async Task AddAsync(
            string objectName,
            BigDecimal objectMass,
            BigDecimal objectDiameter,
            BigDecimal objectTemperature,
            string discoverySourceName,
            DateTime? objectDiscoveryDate = null,
            bool saveChangesImmediately = true)
        {
            //get discovery source optional
            var discoverySource =
                await GetObjectDiscoverySourceByNameAsync(discoverySourceName);

            //try get celestial object
            var celestialObjectOptional =
                await FindCelestialObjectByNameAsync(objectName);

            //set discovery date
            var discoveryDate = objectDiscoveryDate ?? DateTime.UtcNow;

            //create a function to be called with different actions (Add or update)
            Func<Task> addOrUpdate =
                celestialObjectOptional.Any()
                    //if object already exists into database just add an extra discovery source to it
                    ? async () => await AddExtraDiscoverySourceAsync(
                        discoverySource.DiscoverySourceId,
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
                return;
            }

            //commit
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CelestialObject>> GetAllAsync() =>
            await _unitOfWork.CelestialObjectRepo
                .GetAllAsQueryable()
                .Include(x => x.CelestialObjectDiscoveries)
                .ThenInclude(x => x.DiscoverySource)
                .ToListAsync();

        public async Task<IEnumerable<CelestialObject>>
            GetAllObjectByTypeAsync(CelestialObjectType objectType) =>
                await _unitOfWork.CelestialObjectRepo
                    .GetAllAsQueryable()
                    .Where(c => c.Type == objectType)
                    .Include(c => c.CelestialObjectDiscoveries)
                    .ThenInclude(cod => cod.DiscoverySource)
                    .ToListAsync();

        public async Task<Maybe<CelestialObject>>
            FindCelestialObjectByNameAsync(string objectName)
        {
            //get celestial object
            var celestialObject = await _unitOfWork.CelestialObjectRepo
                .GetAllAsQueryable()
                .Where(c => c.Name == objectName)
                .Include(c => c.CelestialObjectDiscoveries)
                .ThenInclude(cod => cod.DiscoverySource)
                .FirstOrDefaultAsync();

            //get an empty maybe or an maybe with object in it
            return celestialObject == null
                ? Maybe.None<CelestialObject>()
                : Maybe.Some(celestialObject);
        }

        public async Task<IEnumerable<CelestialObject>>
            GetObjectsDiscoveredByCountryAsync(string countryName) =>
                await _unitOfWork.CelestialObjectRepo
                    .GetAllAsQueryable()
                    .Include(x => x.CelestialObjectDiscoveries)
                    .ThenInclude(cod => cod.DiscoverySource)
                    .Where(c => c
                        .CelestialObjectDiscoveries
                        .Any(cod => cod.DiscoverySource.StateOwner == countryName))
                    .ToListAsync();
    }
}
