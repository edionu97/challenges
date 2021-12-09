using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using CelestialObjectCatalog.Services.Source;
using Microsoft.AspNetCore.Mvc;

namespace CelestialObjectCatalog.WebApi.Controllers
{
    [ApiController]
    [Route("api/discovery-sources")]
    public class CelestialObjectDiscoverySourceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscoverySourceService _discoverySourceService;

        public CelestialObjectDiscoverySourceController(
            IUnitOfWork unitOfWork,
            IDiscoverySourceService discoverySourceService)
        {
            _unitOfWork = unitOfWork;
            _discoverySourceService = discoverySourceService;
        }

        /// <summary>
        /// This function it is used for getting all the available sources
        /// </summary>
        /// <returns>
        /// A json in the following format:
        ///     {
        ///         "Sources": [
        ///         {
        ///             "name": "ALMA",
        ///             "establishmentDate": "2011-06-15T00:00:00",
        ///             "type": "GroundTelescope",
        ///             "stateOwner": "Chile"
        ///         }
        ///      ]
        ///     }
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSourcesAsync()
        {
            try
            {
                //get available sources
                var sources = await
                    _discoverySourceService
                        .GetAllAsync()
                        .ContinueWith(t =>
                        {
                            //safe to call result since task is already completed
                            var discoverySources = t.Result;

                            //remove the empty list from discovery sources
                            return discoverySources.Select(x =>
                            {
                                x.CelestialObjectDiscoveries = null;
                                return x;
                            });
                        });

                return Ok(new Dictionary<string, IEnumerable<DiscoverySource>>()
                {
                    ["sources"] = sources
                });
            }
            catch (Exception)
            {
                //define the message
                const string message =
                    "There was a problem encountered while retrieving your data, please try again latter";

                //return proper error code
                return Problem(message);
            }
        }

        /// <summary>
        /// This method can be used for adding a single discovery source into database
        /// </summary>
        /// <param name="discoverySource">The discovery source</param>
        /// <returns>Ok response if everything is ok or Problem response otherwise</returns>
        [HttpPost("add")]
        public async Task<IActionResult> 
            AddNewDiscoverySourceAsync([FromBody] DiscoverySource discoverySource)
        {
            //try to save the discovery source
            try
            {
                //make the call
                await _discoverySourceService
                    .AddDiscoverySourceAsync(
                        discoverySource.Name,
                        discoverySource.EstablishmentDate,
                        discoverySource.Type,
                        discoverySource.StateOwner);
            }
            catch (DbException e)
            {
                //get the message
                var message =
                    $"Operation aborted, reason: {e.Message}";

                //return error
                return Problem(message);
            }

            return Ok();
        }


        /// <summary>
        /// This method can be used for adding multiple discovery sources into repository
        /// </summary>
        /// <param name="discoverySources">A list of discovery sources</param>
        /// <returns>Ok response if everything is ok or Problem response otherwise</returns>
        [HttpPost("add/bulk")]
        public async Task<IActionResult>
            AddMultipleDiscoverySourcesAsync([FromBody] IEnumerable<DiscoverySource> discoverySources)
        {
            //try to save the discovery source
            try
            {
                //iterate discovery sources
                foreach (var discoverySource in discoverySources)
                {
                    //make the call, but not save yet
                    await _discoverySourceService
                        .AddDiscoverySourceAsync(
                            discoverySource.Name,
                            discoverySource.EstablishmentDate,
                            discoverySource.Type,
                            discoverySource.StateOwner, 
                            false);
                }

                //commit async
                await _unitOfWork.CommitAsync();
            }
            catch (DbException e)
            {
                //get the message
                var message =
                    $"Instances could not be inserted into database, reason: {e.Message}";

                //return error
                return Problem(message);
            }

            return Ok();
        }
    }
}
