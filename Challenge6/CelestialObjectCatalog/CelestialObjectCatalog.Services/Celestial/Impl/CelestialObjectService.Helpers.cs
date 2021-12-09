using System;
using System.Linq;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Exceptions;
using Deveel.Math;
using Microsoft.Extensions.Logging;

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
                //log info
                if (!optionalSource.Any())
                {
                    //log info
                    _logger?
                        .LogWarning(
                            $"No discovery source with exact name = '[{namePattern}]'" +
                            $" could be located, trying using partial name...");
                }

                //if there is no source present by exact name, try with partial names
                optionalSource = optionalSource.Any()
                    ? optionalSource
                    : await _discoverySourceService
                        .FindDiscoverySourceByPartialName(namePattern);
            }
            catch (InvalidOperationException e)
            {
                //log error
                _logger.LogError(e.Message);
            }

            //perform checking
            if (!optionalSource.Any())
            {
                throw new EntityDoesNotExistException(
                    $"Discovery source name like: [{namePattern}] " +
                    $"could not be found, reason it does not exist in database");
            }

            //return optional source name
            return optionalSource.Single();
        }

        private async Task AddExtraDiscoverySourceAsync(
            Guid discoverySourceId,
            CelestialObject celestialObject,
            DateTime discoveryDate)
        {
            //loge info
            _logger?
                .LogInformation(
                    "Celestial object already inserted into database...," +
                    " trying to add an extra discovery source to object");

            //check if object is already discovered
            var isAlreadyDiscovered =
                celestialObject
                    .CelestialObjectDiscoveries
                    .Any(x => x.DiscoverySourceId == discoverySourceId);

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
                    DiscoverySourceId = discoverySourceId,
                    CelestialObjectId = celestialObject.CelestialObjectId,
                    DiscoveryDate = discoveryDate
                });

            //update the object
            await _unitOfWork
                .CelestialObjectRepo
                .UpdateAsync(celestialObject);
        }

        private async Task CreateCelestialObjectAsync(
            string objectName,
            BigDecimal objectMass,
            BigDecimal objectDiameter,
            BigDecimal objectTemperature,
            DiscoverySource discoverySource,
            DateTime objectDiscoveryDate)
        {
            //log information
            _logger?
                .LogInformation(
                    "Celestial object does not exist in database..., trying to insert it");

            //create celestial object
            var @object = new CelestialObject
            {
                Name = objectName,
                EquatorialDiameter = objectDiameter,
                Mass = objectMass,
                SurfaceTemperature = objectTemperature,
            };

            //set the object's type
            @object.Type =
                _classificationEngine.Classify(@object);

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
            await _unitOfWork
                .CelestialObjectRepo
                .AddAsync(@object);
        }
    }
}
