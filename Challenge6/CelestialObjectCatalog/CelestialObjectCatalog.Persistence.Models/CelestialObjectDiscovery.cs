using System;

namespace CelestialObjectCatalog.Persistence.Models
{
    /// <summary>
    /// Joining class (transforms *:* relation in two 1:* relations)
    /// Need for storing extra payload such as DiscoveryDate
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CelestialObjectDiscovery
    {
        public Guid CelestialObjectId { get; set; }
        public CelestialObject CelestialObject { get; set; }

        public Guid DiscoverySourceId { get; set; }
        public DiscoverySource DiscoverySource { get; set; }

        public  DateTime DiscoveryDate { get; set; }
    }
}
