using System;
using System.Collections.Generic;
using CelestialObjectCatalog.Utility.Attributes;
using CelestialObjectCatalog.Persistence.Models.Enums;
using Deveel.Math;

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
        [UniqueIdentifier]
        public Guid CelestialObjectId { get; set; }

        public string Name { get; set; }

        public BigDecimal Mass { get; set; }

        public BigDecimal EquatorialDiameter { get; set; }

        public BigDecimal SurfaceTemperature { get; set; }

        public CelestialObjectType Type { get; set; }

        public ICollection<CelestialObjectDiscovery> CelestialObjectDiscoveries { get; set; }
            = new List<CelestialObjectDiscovery>();

        /// <summary>
        /// Deconstructs object into given parameters
        /// </summary>
        /// <param name="mass">The mass value</param>
        /// <param name="equatorialDiameter">The diameter value</param>
        /// <param name="surfaceTemperature">The temp value</param>
        public void Deconstruct(
            out BigDecimal mass, 
            out BigDecimal equatorialDiameter, 
            out BigDecimal surfaceTemperature)
        {
            mass = Mass;
            equatorialDiameter = EquatorialDiameter;
            surfaceTemperature = SurfaceTemperature;
        }
    }
}
