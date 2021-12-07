using System;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace CelestialObjectCatalog.Persistence.UnitOfWork.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public IDiscoverySourceRepository DiscoverySourceRepo { get; }

        public IRepository<CelestialObjectDiscovery, Guid> CelestialDiscoveriesRepo { get; }

        public ICelestialObjectRepository CelestialObjectRepo { get; }

        public UnitOfWork(
            DbContext dbContext,
            IDiscoverySourceRepository discoveryRepo,
            IRepository<CelestialObjectDiscovery, Guid> celestialDiscoveriesRepo,
            ICelestialObjectRepository celestialObjectRepo)
        {
            _dbContext = dbContext;
            DiscoverySourceRepo = discoveryRepo;
            CelestialDiscoveriesRepo = celestialDiscoveriesRepo;
            CelestialObjectRepo = celestialObjectRepo;
        }

        public async Task CommitAsync() => await _dbContext.SaveChangesAsync();
    }
}
