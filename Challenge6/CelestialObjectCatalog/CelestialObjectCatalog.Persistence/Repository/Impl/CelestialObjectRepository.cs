using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Models;
using CelestialObjectCatalog.Persistence.Models.Enums;
using CelestialObjectCatalog.Persistence.Repository.Impl.Abstract;
using CelestialObjectCatalog.Utility.Items;
using Microsoft.EntityFrameworkCore;

namespace CelestialObjectCatalog.Persistence.Repository.Impl
{
    public class CelestialObjectRepository : AbstractRepository<CelestialObject, Guid>
    {
        public CelestialObjectRepository(DbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
