using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using CelestialObjectCatalog.Persistence.Exceptions;
using CelestialObjectCatalog.Utility.Items;

namespace CelestialObjectCatalog.Persistence.Repository
{
    public interface IRepository<TEntity, in TId> where TEntity : class, new()
    {
        /// <summary>
        /// This method is used for adding a new element into the repository
        /// </summary>
        /// <param name="entity">The entity that will be added</param>
        /// <returns>A task</returns>
        public Task AddAsync(TEntity entity);

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <exception cref="ArgumentException">If entity does not have a valid unique identifier</exception>
        /// <exception cref="EntityDoesNotExistException">If in repository does not exist any element with that unique identifier</exception>
        /// <returns>A task</returns>
        public Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <param name="entity">The entity that will be deleted</param>
        /// <exception cref="ArgumentException">If entity does not have a valid unique identifier</exception>
        /// <exception cref="EntityDoesNotExistException">If in repository does not exist any element with that unique identifier</exception>
        /// <returns>A task</returns>
        public Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Get all elements from database
        /// </summary>
        /// <returns>A queryable instance which allows further queries on database</returns>
        public IQueryable<TEntity> GetAllAsQueryable(
            params Expression<Func<TEntity, object>>[] columnsToInclude);

        /// <summary>
        /// Gets all elements from database (loads all items in the memory)
        /// </summary>
        /// <returns>A list of items</returns>
        public Task<IEnumerable<TEntity>> GetAllAsync(
            params Expression<Func<TEntity, object>>[] columnsToInclude);

        /// <summary>
        /// Generic find method which filters database against a certain predicate
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <param name="columnsToInclude">A list of functions that describes which extra properties should be included</param>
        /// <returns>A list of items that respects that predicate</returns>
        public Task<IEnumerable<TEntity>> FindAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] columnsToInclude);

        /// <summary>
        /// This method should be used for finding a single object
        /// </summary>
        /// <param name="predicate">The predicate itself</param>
        /// <param name="columnsToInclude">A list of functions that describes which extra properties should be included</param>
        /// <returns>Either an empty maybe or a maybe instance containing a single element</returns>
        public Task<Maybe<TEntity>> FindSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] columnsToInclude);
    }
}
