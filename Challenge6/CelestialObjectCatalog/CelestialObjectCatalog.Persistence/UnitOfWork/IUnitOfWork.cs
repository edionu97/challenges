using System;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace CelestialObjectCatalog.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    { 
        public IDiscoverySourceRepository DiscoverySourceRepo { get; }

        public IRepository<CelestialObjectDiscovery, Guid> CelestialDiscoveriesRepo { get; }

        public ICelestialObjectRepository CelestialObjectRepo { get; }

        public Task CommitAsync();
    }
}
