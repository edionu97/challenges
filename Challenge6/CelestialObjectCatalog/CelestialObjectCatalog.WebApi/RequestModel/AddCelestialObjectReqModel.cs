using System;
using Deveel.Math;

namespace CelestialObjectCatalog.WebApi.RequestModel
{
    public class AddCelestialObjectReqModel
    {
        public string Name { get; set; }

        public string Mass { get; set; }

        public string EquatorialDiameter { get; set; }

        public string SurfaceTemperature { get; set; }

        public DateTime DiscoveryDate { get; set; }

        public string DiscoveredBy { get; set; }
    }
}
