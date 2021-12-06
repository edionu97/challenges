using System;
using System.Collections.Generic;
using CelestialObjectCatalog.Persistence.Models.Enums;

namespace CelestialObjectCatalog.Persistence.Models
{
    /// <summary>
    /// Defines the celestial object entity
    /// Many to many relation with discovery source
    /// Uses CelestialObjectDiscovery to break the *:* relation in 1:* relation
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CelestialObject
    {
        public Guid CelestialObjectId { get; set; }

        public string Name { get; set; }

        public double Mass { get; set; }

        public double EquatorialDiameter { get; set; }

        public double SurfaceTemperature { get; set; }

        public CelestialObjectType Type { get; set; }

        public ICollection<CelestialObjectDiscovery> CelestialObjectDiscoveries { get; set; }
            = new List<CelestialObjectDiscovery>();
    }
}
