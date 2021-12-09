using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CelestialObjectCatalog.WebApi.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class CelestialObjectStatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statistics;

        public CelestialObjectStatisticsController(IStatisticsService statistics)
        {
            _statistics = statistics;
        }

        [HttpGet("sources/max-discoveries/{objectType}")]
        public async Task<IActionResult>
            FindSourcesWithMaxDiscoveriesAsync(CelestialObjectType objectType)
        {
            //execute statistics
            try
            {
                //compute the max discovery statistics
                var maxDiscoveryStatistics = await _statistics
                    .FindDiscoverySourcesWithMostObjectTypeDiscoveriesAsync(objectType)
                    .ContinueWith(prevTask =>
                    {
                        //get previous task result
                        var result = prevTask.Result;

                        //return data in right format
                        return result
                            .GroupBy(x => x.ObjectsDiscovered)
                            .Select(g => new
                            {
                                ObjectDiscoveries = g.Key,
                                Countires = g
                                    .Select(x => x.CountryName)
                                    .OrderBy(x => x)
                            });
                    });

                //return the response
                return Ok(new
                {
                    Stats = maxDiscoveryStatistics
                });
            }
            catch
            {
                //define the message
                const string message =
                    "There was a problem encountered while retrieving your data, please try again latter";

                //return proper error code
                return Problem(message);
            }
        }
    }
}
