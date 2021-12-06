using System;
using System.Collections.Generic;
using CelestialObjectCatalog.Persistence.Models.Enums;

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
        public Guid DiscoverySourceId { get; set; }

        public string Name { get; set; }

        public DateTime EstablishmentDate { get; set; }

        public DiscoverySourceType Type { get; set; }

        public string StateOwner { get; set; }

        public ICollection<CelestialObjectDiscovery> CelestialObjectDiscoveries { get; set; } =
            new List<CelestialObjectDiscovery>();
    }
}
