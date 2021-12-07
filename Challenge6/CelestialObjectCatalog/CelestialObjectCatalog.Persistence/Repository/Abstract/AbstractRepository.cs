using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using CelestialObjectCatalog.Persistence.Constants;
using CelestialObjectCatalog.Persistence.Exceptions;
using CelestialObjectCatalog.Utility.Helpers;
using Microsoft.EntityFrameworkCore;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Persistence.Repository.Abstract
{
    public abstract class AbstractRepository<TEntity, TId> :
        IRepository<TEntity, TId> where TEntity : class, new()
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected DbSet<TEntity> DbSet { get; }

        // ReSharper disable once MemberCanBePrivate.Global
        protected DbContext DbContext { get; }

        protected AbstractRepository(DbContext dbContext)
        {
            //set db context
            DbContext = dbContext;

            //create the db set
            DbSet = dbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            //add entity
            await DbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            //check if entity exists
            if (!await CheckIfEntityExistsAsync(entity))
            {
                throw new EntityDoesNotExistException(
                    MessageConstants.EntityCouldNotBeDeletedMessage);
            }

            //remove entity
            DbSet.Remove(entity);
        }
        public async Task UpdateAsync(TEntity entity)
        {
            //check if entity exists
            if (!await CheckIfEntityExistsAsync(entity))
            {
                throw new EntityDoesNotExistException(
                    MessageConstants.EntityCouldNotBeUpdatedMessage);
            }

            //attach entity
            DbSet.Attach(entity);

            //modify the entry state to update
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<Maybe<TEntity>> FindByIdAsync(params TId[] keyValues)
        {
            //get primary key values
            var primaryKeyValues = keyValues
                .Select(x => (object)x)
                .ToArray();

            //get the entity
            var item = await DbSet.FindAsync(primaryKeyValues);

            //if item is null return empty maybe
            return item is null
                ? Maybe.None<TEntity>()
                : Maybe.Some(item);
        }

        public async Task<Maybe<IEnumerable<TEntity>>> FindAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            //get items
            var items = DbSet.Where(predicate);

            //if there are any items in list return 
            return await items.AnyAsync()
                ? Maybe.Some<IEnumerable<TEntity>>(await items.ToListAsync())
                : Maybe.None<IEnumerable<TEntity>>();
        }

        public IQueryable<TEntity> GetAllAsQueryable() => DbSet.AsQueryable();

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await DbSet.ToListAsync();
       

        /// <summary>
        /// This method should be used for checking if an entity exists in repository
        /// </summary>
        /// <param name="entity">The entity that is checked</param>
        /// <exception cref="ArgumentException">If the TEntity does not have a valid identifier</exception>
        /// <returns>True if entity exists or false otherwise</returns>
        protected virtual async Task<bool> CheckIfEntityExistsAsync(TEntity entity)
        {
            //try to get unique ids optional
            var uniqueIdsOptional =
                await ReflectionHelpers
                    .GetObjectOrderedUniqueIdentifiersAsync<TId>(entity);

            //check if unique identifiers could be extracted
            if (!uniqueIdsOptional.Any())
            {
                throw new ArgumentException(
                    string.Format(MessageConstants
                        .UniqueIdsCannotBeComputedMessage, typeof(TId).FullName));
            }

            //get unique ids
            var keyComponents = uniqueIdsOptional
                .Single()
                .ToArray();

            //get item by keyValues
            var maybeEntity =
                await FindByIdAsync(keyComponents);

            //return true if the element could be found or false otherwise 
            return maybeEntity.Any();
        }
    }
}
