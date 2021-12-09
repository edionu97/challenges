using System;
using System.Collections.Generic;
using System.Linq;
using Deveel.Math;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CelestialObjectCatalog.Utility.Helpers;
using CelestialObjectCatalog.Services.Celestial;
using CelestialObjectCatalog.WebApi.RequestModel;
using CelestialObjectCatalog.Persistence.Exceptions;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Persistence.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace CelestialObjectCatalog.WebApi.Controllers
{
    [Route("api/celestial-objects")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ICelestialObjectService _celestialObjectService;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<CelestialObjectController> _logger;

        public CelestialObjectController(
            ICelestialObjectService celestialObjectService,
            IUnitOfWork unitOfWork,
            ILogger<CelestialObjectController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _celestialObjectService = celestialObjectService;
        }

        /// <summary>
        /// This method executes data filtering. If no query params => get all
        /// </summary>
        /// <param name="filteringOptions">A class which contains multiple parameters</param>
        /// <returns>
        ///  BadRequest => iif more than one query parameter is specified
        /// </returns>
        [HttpGet]
        public async Task<IActionResult>
            FilterObjectsAsync([FromQuery] CelestialFilterQueryReqModel filteringOptions)
        {
            //get not null properties
            var notNullProperties =
                ReflectionHelpers
                    .GetNotNullPropertyValues(filteringOptions)
                    .ToList();

            //get all
            if (!notNullProperties.Any())
            {
                return BuildFindResponse(
                    await _celestialObjectService.GetAllAsync());
            }

            //ensure we have only one filtering
            if (notNullProperties.Count > 1)
            {
                //get property names
                var propNames = string
                    .Join(
                        ',', 
                        notNullProperties.Select(x => x.Name));

                //write not null properties
                _logger
                    .LogError(
                        $"More than one filtering property encountered: [{propNames}]");

                return BadRequest("More than one filter parameter encountered, use one single filter parameter");
            }

            //try to get all items from repository
            try
            {
                //deconstruct the filtering conditions
                var (propName, propValue) = notNullProperties.Single();

                //apply proper filter
                return propName switch
                {
                    //handle the filter by type query
                    nameof(filteringOptions.Type) =>
                        BuildFindResponse(
                            await _celestialObjectService
                                .GetAllObjectByTypeAsync((CelestialObjectType)propValue)),

                    //handle filter by country query
                    nameof(filteringOptions.ByCountry) =>
                        BuildFindResponse(
                            await _celestialObjectService
                                    .GetObjectsDiscoveredByCountryAsync((propValue as string)?.Trim())),

                    //handle filter by name (returns a single object)
                    nameof(filteringOptions.Named) =>
                        BuildFindResponse(
                            await _celestialObjectService
                                .FindCelestialObjectByNameAsync((propValue as string)?.Trim())),

                    //default case
                    _ => throw new ArgumentOutOfRangeException()
                };
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
        /// Helper method for building the find response
        /// </summary>
        /// <param name="celestialObjects">A element representing the payload</param>
        /// <returns>An Ok action result</returns>
        private IActionResult 
            BuildFindResponse(object celestialObjects) => Ok(new { celestialObjects });

        /// <summary>
        /// This methods add a single celestial object into repository
        /// </summary>
        /// <param name="addRequestModel">The request model</param>
        /// <returns>Ok if everything is ok, Problem is there is any exception or NotFound if there does not exist any discouvery source with given name</returns>
        [HttpPost("add")]
        public async Task<IActionResult>
            AddCelestialObjectAsync([FromBody] AddCelestialObjectReqModel addRequestModel)
        {
            try
            {
                //try to add resource
                await _celestialObjectService
                    .AddAsync(
                        addRequestModel.Name.Trim(),
                        BigDecimal.Parse(addRequestModel.Mass),
                        BigDecimal.Parse(addRequestModel.EquatorialDiameter),
                        BigDecimal.Parse(addRequestModel.SurfaceTemperature),
                        addRequestModel.DiscoveredBy.Trim(),
                        addRequestModel.DiscoveryDate);
            }
            catch (EntityDoesNotExistException e)
            {
                //treat not found exception
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

            //return 200 OK
            return Ok();
        }

        /// <summary>
        /// This methods adds multiple celestial objects into repository
        /// </summary>
        /// <param name="addRequestModels">A list with multiple request items</param>
        /// <returns>Ok if everything is ok, Problem is there is any exception or NotFound if there does not exist any discouvery source with given name</returns>
        [HttpPost("add/bulk")]
        public async Task<IActionResult>
            AddCelestialObjectsAsync([FromBody] IEnumerable<AddCelestialObjectReqModel> addRequestModels)
        {
            try
            {
                //add multiple items
                foreach (var addRequestModel in addRequestModels)      
                {
                    //try to add resource
                    await _celestialObjectService
                        .AddAsync(
                            addRequestModel.Name.Trim(),
                            BigDecimal.Parse(addRequestModel.Mass),
                            BigDecimal.Parse(addRequestModel.EquatorialDiameter),
                            BigDecimal.Parse(addRequestModel.SurfaceTemperature),
                            addRequestModel.DiscoveredBy.Trim(),
                            addRequestModel.DiscoveryDate,
                            false);
                }

                //multiple additions
                await _unitOfWork.CommitAsync();
            }
            catch (EntityDoesNotExistException e)
            {
                //treat not found exception
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

            //return 200 OK
            return Ok();
        }
    }
}
