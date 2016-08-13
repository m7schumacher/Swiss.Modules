using System;
using System.Collections.Generic;
using System.Linq;

namespace Swiss
{
    public static class IDictionaryExtensions
    {
        #region Insertion

        /// <summary>
        /// Method ensures an add only occurs if the key is not yet present in the dictionary
        /// </summary>
        public static void AddIfNotContainsKey<T, K>(this IDictionary<T, K> dictionary, T key, K value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Method adds a given entry to the dictionary if no such key exists, or updates the value of that key if it does
        /// </summary>
        public static void AddOrUpdate<T, K>(this IDictionary<T, K> dictionary, T key, K value)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, value);
            else dictionary[key] = value;
        }

        /// <summary>
        /// Method performs an add or update if the given key and value satisfy a condition
        /// </summary>
        public static void AddOrUpdateIf<T,K>(this IDictionary<T,K> dictionary, T key, K value, Func<KeyValuePair<T,K>, bool> predicate)
        {
            if(predicate(new KeyValuePair<T, K>(key, value)))
            {
                dictionary.AddOrUpdate(key, value);
            }
        }

        /// <summary>
        /// Method adds the contents of a given dictionary to this one
        /// </summary>
        public static void AddRange<T, K>(this IDictionary<T, K> dictionary, IDictionary<T, K> other)
        {
            other.ForEach(pair => dictionary.AddIfNotContainsKey(pair.Key, pair.Value));
        }

        /// <summary>
        /// Method performs and add or update on contents of a given dictionary to this one
        /// </summary>
        public static void AddOrUpdateRange<T, K>(this IDictionary<T, K> dictionary, IDictionary<T, K> other)
        {
            other.ForEach(pair => dictionary.AddOrUpdate(pair.Key, pair.Value));
        }

        /// <summary>
        /// Method either creates new entry or increments existing entry in this dictionary, useful for counting
        /// </summary>
        public static void IncrementOrAddNew<T>(this IDictionary<T, int> dictionary, T key, int value = 1)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, 1);
            else dictionary[key] += value;
        }

        /// <summary>
        /// Method performs an increment or add new on an enumerable to elements
        /// </summary>
        public static void IncrementOrAddNewRange<T>(this IDictionary<T, int> dictionary, IEnumerable<T> keys)
        {
            keys.ForEach(key => dictionary.IncrementOrAddNew(key));
        }

        /// <summary>
        /// Method either creates new entry or increments existing entry in this dictionary, useful for counting
        /// </summary>
        public static void IncrementOrAddNew<T>(this IDictionary<T, double> dictionary, T key, double value = 1)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, 1);
            else dictionary[key] += value;
        }

        /// <summary>
        /// Method combines an enumerable of dictionaries with this one, forming a large dictionary
        /// </summary>
        public static void CombineWithAnother<T, K>(this IDictionary<T, K> dictionary, IEnumerable<IDictionary<T, K>> others)
        {
            others.ForEach(dict => dictionary.AddRange(dict));
        }

        #endregion

        #region Metrics

        /// <summary>
        /// Method returns the key with the largest value
        /// </summary>
        public static T GetKeyWithLargestValue<T,K>(this IDictionary<T,K> dictionary) where K : IComparable<K>
        {
            return dictionary.Reverse().Aggregate((l, r) => l.Value.CompareTo(r.Value) > 0 ? l : r).Key;
        }

        /// <summary>
        /// Method returns a group of keys with the largest value, can handle ties
        /// </summary>
        public static List<T> GetKeysWithLargestValues<T, K>(this IDictionary<T, K> dictionary) where K : IComparable<K>
        {
            var ordered = dictionary.SortByValueDescending(val => val);

            List<T> keys = new List<T>();

            int index = 1;
            var previous = ordered.First().Value;
            keys.Add(ordered.First().Key);

            while (index < ordered.Count())
            {
                var max = ordered.ElementAt(index).Value;

                if (max.CompareTo(previous) == 0)
                {
                    keys.Add(ordered.ElementAt(index).Key);
                    index++;
                } 
                else break;
            }

            return keys;
        }

        /// <summary>
        /// Method gathers aggregate of values from this dictionary if said values are enumerables
        /// </summary>
        public static List<K> GatherAggregateOfValues<T, K>(this IDictionary<T, K[]> dictionary)
        {
            List<K> values = new List<K>();
            dictionary.Keys.ForEach(key => values.AddRange(dictionary[key]));

            return values;
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Method sorts this dictionary by its keys in a descending fashion
        /// </summary>
        public static Dictionary<T, K> SortByKeyDescending<T, K, U>(this IDictionary<T, K> dictionary, Func<T, U> field)
        {
            return dictionary.SortByKey(field).Reverse().ToDictionary();
        }

        /// <summary>
        /// Method sorts this dictionary by its keys
        /// </summary>
        public static Dictionary<T,K> SortByKey<T, K, U>(this IDictionary<T, K> dictionary, Func<T, U> field)
        {
            var keys = dictionary.Keys.OrderBy(key => field(key));
            return keys.ToList().ToDictionary(key => key, key => dictionary[key]);
        }

        /// <summary>
        /// Method sorts this dictionary by its values in a descending fashion
        /// </summary>
        public static Dictionary<T, K> SortByValueDescending<T, K, U>(this IDictionary<T, K> dictionary, Func<K, U> field)
        {
            return dictionary.SortByValue(field).Reverse().ToDictionary();
        }

        /// <summary>
        /// Method sorts this dictionary by its values
        /// </summary>
        public static Dictionary<T, K> SortByValue<T, K, U>(this IDictionary<T, K> dictionary, Func<K, U> field)
        {
            var sorted = from entry in dictionary
                         orderby field(entry.Value)
                         select entry;

            return sorted.ToList().ToDictionary();
        }

        #endregion

        #region Filtering

        /// <summary>
        /// Method returns only key value pairs with a value that satisfies a given condition
        /// </summary>
        public static IDictionary<T,K> FilterOnValue<T,K>(this IDictionary<T,K> dictionary, Func<K,bool> predicate)
        {
            return dictionary.Where(val => predicate(val.Value)).ToDictionary();
        }

        /// <summary>
        /// Method returns only key value pairs with a key that satisfies a given condition
        /// </summary>
        public static IDictionary<T, K> FilterOnKey<T, K>(this IDictionary<T, K> dictionary, Func<T, bool> predicate)
        {
            return dictionary.Where(val => predicate(val.Key)).ToDictionary();
        }

        #endregion
    }
}
