using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CelestialObjectCatalog.Persistence.Constants;
using CelestialObjectCatalog.Utility.Helpers;
using CelestialObjectCatalog.Utility.Items;
using Microsoft.EntityFrameworkCore;

namespace CelestialObjectCatalog.Persistence.Repository.Impl.Abstract
{
    public abstract partial class AbstractRepository<TEntity, TId>
    {
        /// <summary>
        /// This function helps in including properties in models
        /// </summary>
        /// <param name="queryable">The queryable instance</param>
        /// <param name="columnsToInclude">A list of expressions which contains info about columns that will be included</param>
        /// <returns>A modified queryable instance</returns>
        protected static IQueryable<TEntity> IncludeFields(
            IQueryable<TEntity> queryable,
            params Expression<Func<TEntity, object>>[] columnsToInclude)
        {
            //declare property regex
            var propertyNameRegex = new Regex(@"\.(?<propName>\w+)");

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var propertyNameExpr in columnsToInclude)
            {
                //get the match
                var match = propertyNameRegex
                    .Match(propertyNameExpr.ToString());

                //get property name
                var properName = match
                    .Groups["propName"]
                    .Value;

                //include property name in queryable
                queryable = queryable.Include(properName);
            }

            //return queryable
            return queryable;
        }

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

        

        /// <summary>
        /// Check if an object with a given id exists in database
        /// </summary>
        /// <param name="keyValues">The key values</param>
        /// <returns>An empty maybe if object does not exist or an maybe of element otherwise</returns>
        private async Task<Maybe<TEntity>> FindByIdAsync(params TId[] keyValues)
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
    }
}
