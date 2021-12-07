using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace CelestialObjectCatalog.Utility.Items
{
    public class Maybe<TObject> : IEnumerable<TObject>
    {
        private readonly IEnumerable<TObject> _innerElements;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="innerElements"></param>
        private Maybe(IEnumerable<TObject> innerElements)
        {
            _innerElements = innerElements;
        }

        /// <summary>
        /// This function can be used for creating a maybe containing a single element
        /// </summary>
        /// <param name="element">The element itself</param>
        /// <returns>A maybe of that element</returns>
        /// <exception cref="ArgumentNullException">If null is passed as parameter</exception>
        internal static Maybe<TObject> Of(TObject element)
        {
            //null check
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            //create maybe of element
            return new Maybe<TObject>(new[] { element });
        }

        /// <summary>
        /// This function creates an empty maybe
        /// </summary>
        /// <returns>A empty maybe</returns>
        internal static Maybe<TObject> Empty()
            => new(Enumerable.Empty<TObject>());

        public IEnumerator<TObject> GetEnumerator() 
            => _innerElements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class Maybe
    {
        public static Maybe<TObject> Some<TObject>(TObject item) => Maybe<TObject>.Of(item);

        public  static Maybe<TObject> None<TObject>() => Maybe<TObject>.Empty();
    }
}
