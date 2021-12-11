using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.WebApi.RequestModel
{
    public class CelestialFilterQueryReqModel
    {
        public CelestialObjectType? Type { get; set; }

        public string Named { get; set; }

        public string ByCountry { get; set; }
    }
}
