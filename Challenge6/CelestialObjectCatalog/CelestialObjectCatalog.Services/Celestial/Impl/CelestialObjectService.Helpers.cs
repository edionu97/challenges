using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Exceptions;
using CelestialObjectCatalog.Persistence.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CelestialObjectCatalog.Services.Celestial.Impl
{
    public partial class CelestialObjectService
    {
        private async Task<DiscoverySource> GetObjectDiscoverySourceByNameAsync(string namePattern)
        {
            //get the optional source by it's name
            var optionalSource = await _discoverySourceService
                .FindDiscoverySourceAsync(namePattern);

            try
            {
                //if there is no source present by exact name, try with partial names
                optionalSource = optionalSource.Any()
                    ? optionalSource
                    : await _discoverySourceService
                        .FindDiscoverySourceByPartialName(namePattern);
            }
            catch (InvalidOperationException)
            {
                //ignore
            }

            //perform checking
            if (!optionalSource.Any())
            {
                throw new EntityDoesNotExistException(
                    $"Discovery source name like:[{namePattern}] " +
                    $"could not be found, reason it does not exist in database");
            }

            //return optional source name
            return optionalSource.Single();
        }

        private async Task AddExtraDiscoverySourceAsync(
            DiscoverySource discoverySource,
            CelestialObject celestialObject,
            DateTime discoveryDate)
        {
            //check if object is already discovered
            var isAlreadyDiscovered =
                celestialObject
                    .CelestialObjectDiscoveries
                    .Any(x => x.DiscoverySourceId == discoverySource.DiscoverySourceId);

            //do nothing is object is discovered
            if (isAlreadyDiscovered)
            {
                throw new EntityAlreadyExistsException(
                    "The new discovery source could not be added, reason: record already exists");
            }

            //add new discovery
            celestialObject
                .CelestialObjectDiscoveries
                .Add(new CelestialObjectDiscovery
                {
                    DiscoverySourceId = discoverySource.DiscoverySourceId,
                    CelestialObjectId = celestialObject.CelestialObjectId,
                    DiscoveryDate = discoveryDate
                });

            //update the object
            await _celestialObjectRepository.UpdateAsync(celestialObject);
        }

        private async Task CreateCelestialObjectAsync(
            string objectName,
            double objectMass,
            double objectDiameter,
            double objectTemperature,
            DiscoverySource discoverySource,
            DateTime objectDiscoveryDate)
        {
            //create celestial object
            var @object = new CelestialObject
            {
                Name = objectName,
                EquatorialDiameter = objectDiameter,
                Mass = objectMass,
                SurfaceTemperature = objectTemperature,
                Type = CelestialObjectType.Unknown
            };

            //add discovery
            @object
                .CelestialObjectDiscoveries
                .Add(new CelestialObjectDiscovery
                {
                    CelestialObject = @object,
                    DiscoverySource = discoverySource,
                    DiscoveryDate = objectDiscoveryDate
                });

            //add object into database
            await _celestialObjectRepository.AddAsync(@object);
        }

    }
}
