using System;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace CelestialObjectCatalog.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    { 
        public IRepository<DiscoverySource, Guid> DiscoverySourceRepo { get; }

        public IRepository<CelestialObjectDiscovery, Guid> CelestialDiscoveriesRepo { get; }

        public IRepository<CelestialObject, Guid> CelestialObjectRepo { get; }

        public Task CommitAsync();
    }
}
