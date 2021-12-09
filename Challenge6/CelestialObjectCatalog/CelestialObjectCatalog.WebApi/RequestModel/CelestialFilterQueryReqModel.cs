using Microsoft.AspNetCore.Mvc;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.WebApi.RequestModel
{
    [BindProperties]
    public class CelestialFilterQueryReqModel
    {
        public CelestialObjectType? Type { get; set; }

        public string Named { get; set; }

        public string ByCountry { get; set; }
    }
}
