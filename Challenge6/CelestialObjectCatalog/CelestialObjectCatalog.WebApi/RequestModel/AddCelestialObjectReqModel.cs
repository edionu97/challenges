using System;
using System.ComponentModel.DataAnnotations;
using CelestialObjectCatalog.WebApi.Validation.Attributes;

namespace CelestialObjectCatalog.WebApi.RequestModel
{
    public class AddCelestialObjectReqModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [IsBigDecimal]
        public string Mass { get; set; }

        [Required]
        [IsBigDecimal]
        public string EquatorialDiameter { get; set; }

        [Required]
        [IsBigDecimal]
        public string SurfaceTemperature { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DiscoveryDate { get; set; }

        [Required]
        public string DiscoveredBy { get; set; }
    }
}
