using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Persistence.Exceptions;

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
        public IQueryable<TEntity> GetAllAsQueryable();

        /// <summary>
        /// Gets all elements from database (loads all items in the memory)
        /// </summary>
        /// <returns>A list of items</returns>
        public Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Find an element by it's key or composite key
        /// </summary>
        /// <param name="keyValues">Key elements</param>
        /// <returns>An empty maybe if there is no element, or a maybe containing desired element</returns>
        public Task<Maybe<TEntity>> 
            FindByIdAsync(params TId[] keyValues);

        /// <summary>
        /// Generic find method which filters database against a certain predicate
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <returns>A list of items that respects that predicate</returns>
        public Task<Maybe<IEnumerable<TEntity>>> FindAsync(
            Expression<Func<TEntity, bool>> predicate);
    }
}
