using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using CelestialObjectCatalog.Utility.Items;
using CelestialObjectCatalog.Utility.Attributes;

namespace CelestialObjectCatalog.Utility.Helpers
{
    public static class ReflectionHelpers
    {
        /// <summary>
        /// This function should be used for getting objects unique identifiers in the order of declaration
        /// </summary>
        /// <typeparam name="TResult">Type of unique identifiers value</typeparam>
        /// <param name="object">The object instance</param>
        /// <returns>
        ///     An empty maybe if the object is null or it has some unique properties that cannot be converted to TResult
        ///     A maybe with a list of values representing the unique ids values
        /// </returns>
        public static Task<Maybe<ICollection<TResult>>>
            GetObjectOrderedUniqueIdentifiersAsync<TResult>(object @object) =>
                Task.Run(() =>
                {
                    //declare the list
                    List<(int Order, object Value)> propValues = new();

                    //check object for null
                    if (@object is null)
                    {
                        //return empty maybe
                        return Task
                          .FromResult(Maybe.None<ICollection<TResult>>());
                    }

                    //iterate properties
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var propertyInfo in @object.GetType().GetProperties())
                    {
                        //get property attribute
                        var desiredAttribute =
                          propertyInfo.GetCustomAttribute<UniqueIdentifierAttribute>();

                        //null check
                        if (desiredAttribute is null)
                        {
                            continue;
                        }

                        //add values
                        propValues.Add((desiredAttribute.Order, propertyInfo.GetValue(@object)));
                    }

                    //get the ordered properties values
                    var orderedPropertiesValues =
                      propValues
                          .OrderBy(x => x.Order)
                          .Select(x => x.Value)
                          .ToList();


                    //check condition
                    if (orderedPropertiesValues.Any(x => x is not TResult))
                    {
                        //return empty task
                        return Task
                          .FromResult(Maybe.None<ICollection<TResult>>());
                    }

                    //order values by their line
                    var values = orderedPropertiesValues
                      .Cast<TResult>()
                      .ToList();

                    //return the result
                    return Task
                      .FromResult(Maybe
                          .Some<ICollection<TResult>>(values));
                });
    }
}
