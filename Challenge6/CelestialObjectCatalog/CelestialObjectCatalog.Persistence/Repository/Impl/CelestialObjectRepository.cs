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
    public class CelestialObjectRepository : AbstractRepository<CelestialObject, Guid>, ICelestialObjectRepository
    {
        public CelestialObjectRepository(DbContext dbContext) 
            : base(dbContext)
        {
        }

        public Task<IEnumerable<CelestialObject>> 
            GetAllObjectsOfTypeAsync(CelestialObjectType objectType) => FindAsync(x => x.Type == objectType);

        public async Task<Maybe<CelestialObject>> GetObjectByNameAsync(string objectName)
        {
            //get all objects with desired name
            var objects =
                (await FindAsync(x => x.Name == objectName)).ToList();
            
            //if there is any object return maybe of object otherwise return empty maybe
            return objects.Any() 
                ? Maybe.Some(objects.First()) 
                : Maybe.None<CelestialObject>();
        }

        public Task<IEnumerable<CelestialObject>> 
            GetAllObjectDiscoveredByAsync(string countryName)
        {
            return null;
        }
    }
}
