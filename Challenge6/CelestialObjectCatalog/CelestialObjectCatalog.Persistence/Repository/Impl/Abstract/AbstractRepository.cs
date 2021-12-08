using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CelestialObjectCatalog.Persistence.Constants;
using CelestialObjectCatalog.Persistence.Exceptions;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Persistence.Repository.Impl.Abstract
{
    public abstract partial class AbstractRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class, new()
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

        public async Task<Maybe<TEntity>> FindSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] columnsToInclude)
        {
            //get all items
            var items =
                (await FindAllAsync(predicate, columnsToInclude)).ToList();
            
            //check the length
            if (items.Count > 1)
            {
                throw new InvalidOperationException(
                    "There is more than one matching for given pattern." +
                    $" Restrict the pattern:'[{predicate}]' in order to obtain a single result");
            }

            //return either an empty maybe or nothing
            return items.Count == 0
                ? Maybe.None<TEntity>()
                : Maybe.Some(items[0]);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] columnsToInclude)
        {
            //get items and also include fields
            var items =
                IncludeFields(
                    DbSet.Where(predicate),
                    columnsToInclude);

            //if there are any items in list return 
            return await items.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            params Expression<Func<TEntity, object>>[] columnsToInclude)
                => await IncludeFields(DbSet, columnsToInclude).ToListAsync();

        public IQueryable<TEntity> GetAllAsQueryable(
            params Expression<Func<TEntity, object>>[] columnsToInclude) 
                => IncludeFields(DbSet, columnsToInclude);
    }
}
