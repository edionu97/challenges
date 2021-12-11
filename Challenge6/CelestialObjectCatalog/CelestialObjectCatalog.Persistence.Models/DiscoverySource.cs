using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Utility.Attributes;

namespace CelestialObjectCatalog.Persistence.Models
{
    /// <summary>
    /// Discovery source entity
    /// Many to many with celestial object
    /// Uses CelestialObjectDiscovery to break the *:* relation in 1:* relation
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DiscoverySource
    {
        [JsonIgnore]
        [UniqueIdentifier]
        public Guid DiscoverySourceId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EstablishmentDate { get; set; }

        [Required]
        public DiscoverySourceType Type { get; set; }

        [Required]
        public string StateOwner { get; set; }

        [JsonIgnore]
        public ICollection<CelestialObjectDiscovery> CelestialObjectDiscoveries { get; set; } =
            new List<CelestialObjectDiscovery>();
    }
}
