using System;
using System.Collections.Generic;
using System.Linq;

namespace Swiss
{
    public static class ICollectionExtensions
    {
    
        /// <summary>
        /// Method adds an enumerable of elements to the collection
        /// </summary>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            values.ForEach(val => collection.Add(val));
        }

        /// <summary>
        /// Method adds only elements of an enumerable that satisfy a condition to the collection
        /// </summary>
        public static void AddWhere<T>(this ICollection<T> collection, Func<T, bool> predicate, IEnumerable<T> values)
        {
            collection.AddRange(values.Where(val => predicate(val)));
        }

        /// <summary>
        /// Method adds an element to the collection only if a given condition is ture
        /// </summary>
        public static void AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T value)
        {
            if (predicate(value))
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// Method adds a given element to the collection if the collection does not already contain that element
        /// </summary>
        public static void AddIfNotContains<T>(this ICollection<T> collection, T value)
        {
            if (!collection.Contains(value))
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// Method adds to the collection from a given list of elements where elements are not yet contained in the collection
        /// </summary>
        public static void AddWhereNotContains<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            values.ForEach(val => collection.AddIfNotContains(val));
        }

        /// <summary>
        /// Method removes an enumerable of elements from the collection
        /// </summary>
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            values.ForEach(val => collection.Remove(val));
        }

        /// <summary>
        /// Method removes all elements from the collection which do not satisfy a condition
        /// </summary>
        public static List<T> RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            return collection.Where(elem => !predicate(elem)).ToList();
        }
    }
}
