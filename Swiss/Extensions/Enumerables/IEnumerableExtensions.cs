using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Swiss
{
    public static class IEnumerableExtensions
    {
        #region General

        #region Manipulation

        /// <summary>
        /// Method enables a lambda style foreach to be run on Enumerable types
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> act)
        {
            foreach (T val in collection)
            {
                act(val);
            }
        }

        /// <summary>
        /// Method mutatues elements of the enumerable that satisfy a given condition - can be run in parallel
        /// </summary>
        public static IEnumerable<T> AlterWhere<T>(this IEnumerable<T> collection, Func<T, bool> predicate, Func<T, T> mutator, bool parallel = false)
        {
            if (parallel)
            {
                Func<T, T> action = (T value) => predicate(value) ? mutator(value) : value;
                return ParallelUtility.ExecuteInParallel(collection, action);
            }
            else
            {
                List<T> altered = new List<T>();
                collection.ForEach(element => altered.Add(predicate(element) ? mutator(element) : element));

                return altered;
            }
        }

        /// <summary>
        /// Method returns the first element in the enumerable that satisifies a condition, or the default of tha type
        /// </summary>
        public static K HasOrDefault<T,K>(this IEnumerable<T> collection, Func<T,bool> predicate, Func<T,K> value, K def)
        {
            var match = collection.FirstOrDefault(elem => predicate(elem));
            return match != null ? value(match) : def;
        }

        /// <summary>
        /// Method gets all of the distinct values of a given field in the enumerable
        /// </summary>
        public static IEnumerable<K> GetDistinctValuesOfField<T, K>(this IEnumerable<T> collection, Func<T, K> field)
        {
            return collection.GroupBy(field).Select(e => field(e.First()));
        }

        /// <summary>
        /// Method gets all of the distinct values of a given field in the enumerable along with how many times those values appear
        /// </summary>
        public static Dictionary<K, int> GetCountsOfDistinctFields<T, K>(this IEnumerable<T> collection, Func<T, K> field)
        {
            var pairs = collection.GroupBy(field).Select(e => new KeyValuePair<K, int>(field(e.First()), e.Count())).ToList();
            return pairs.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        /// Method returns the index of the first element in the enumerable that satisfies a condition
        /// </summary>
        public static int FirstIndexOf<T>(this IEnumerable<T> collection, Func<T, bool> condition)
        {
            int index = -1;

            for (int i = 0; i < collection.Count(); i++)
            {
                T element = collection.ElementAt(i);

                if (element != null && condition(element))
                {
                    return i;
                }
            }

            return index;
        }

        /// <summary>
        /// Method returns the index of the first element in the enumerable that equals a given element
        /// </summary>
        public static int FirstIndexOf<T>(this IEnumerable<T> collection, T value)
        {
            return collection.FirstIndexOf(elem => elem.Equals(value));
        }

        /// <summary>
        /// Method returns the index of the last element in the enumerable that satisfies a condition
        /// </summary>
        public static int LastIndexOf<T>(this IEnumerable<T> collection, Func<T, bool> condition)
        {
            return collection.Reverse().FirstIndexOf(condition);
        }

        /// <summary>
        /// Method returns the index of the last element in the enumerable that equals a given element
        /// </summary>
        public static int LastIndexOf<T>(this IEnumerable<T> collection, T value)
        {
            return collection.LastIndexOf(elem => elem.Equals(value));
        }

        /// <summary>
        /// Method returns whether or not this enumerable has any elements
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection.Count() == 0;
        }

        /// <summary>
        /// Method returns whether or not this enumerable contains each one of the given values
        /// </summary>
        public static bool ContainsAll<T>(this IEnumerable<T> collection, IEnumerable<T> values)
        {
            return values.All(val => collection.Contains(val));
        }

        /// <summary>
        /// Method returns whether or not this enumerable contains any of the given values
        /// </summary>
        public static bool ContainsAny<T>(this IEnumerable<T> collection, IEnumerable<T> values)
        {
            return values.Any(val => collection.Contains(val));
        }

        /// <summary>
        /// Method returns whether or not all element of this enumerable are null
        /// </summary>
        public static bool AreAllNull<T>(this IEnumerable<T> collection)
        {
            return collection.All(elem => elem == null);
        }

        /// <summary>
        /// Method returns whether or not any element of this enumerable are null
        /// </summary>
        public static bool AreAnyNull<T>(this IEnumerable<T> collection)
        {
            return collection.Any(elem => elem == null);
        }

        #endregion

        #region Metrics

        /// <summary>
        /// Method returns the mean value of a given field in this enumerable
        /// </summary>
        public static double MeanOfFields<T>(this IEnumerable<T> elements, Func<T, dynamic> func)
        {
            double count = elements.Count();
            double sum = elements.Sum(elem => func(elem));

            return sum / count;
        }

        /// <summary>
        /// Method returns the collective product of a given field in this enumerable
        /// </summary>
        public static double ProductOfFields<T>(this IEnumerable<T> elements, Func<T, dynamic> func)
        {
            double product = 1;
            elements.ToList().ForEach(elem => product *= func(elem));
            return product;
        }

        /// <summary>
        /// Method returns the standard deviation of a given field in this enumerable
        /// </summary>
        public static double StandardDeviationOfFields<T>(this IEnumerable<T> elements, Func<T, dynamic> func)
        {
            double mean = MeanOfFields(elements, func);
            double sqdDiffs = 0;

            elements.ToList().ForEach(elem => sqdDiffs += Math.Pow(func(elem) - mean, 2));

            return Math.Sqrt((sqdDiffs / elements.Count()));
        }

        /// <summary>
        /// Method returns the element with the maximum value of a given field in this enumerable
        /// </summary>
        public static T MaxOfField<T>(this IEnumerable<T> collection, Func<T, dynamic> function)
        {
            return collection.Count() > 0 ? collection.Aggregate((i, j) => function(i) > function(j) ? i : j) : default(T);
        }

        /// <summary>
        /// Method returns the element with the minimum value of a given field in this enumerable
        /// </summary>
        public static T MinOfField<T>(this IEnumerable<T> collection, Func<T, dynamic> function)
        {
            return collection.Count() > 0 ? collection.Aggregate((i, j) => function(i) < function(j) ? i : j) : default(T);
        }

        #endregion

        #region Generation

        /// <summary>
        /// Method transforms this enumerable of key value pairs into a dictionary, handling possible exceptions as well
        /// </summary>
        public static Dictionary<T,K> ToDictionary<T,K>(this IEnumerable<KeyValuePair<T,K>> pairs)
        {
            return pairs.Where(pr => pr.Key != null && pr.Value != null)//watch for nulls
                        .GroupBy(pr => pr.Key)//handle possible duplicate keys by taking first entry
                        .ToDictionary(pa => pa.Key, pa => pa.First().Value);
        }

        /// <summary>
        /// Method combines the contents of this enumerable with that of a given enumerable
        /// </summary>
        public static IEnumerable<T> CombineWith<T>(this IEnumerable<T> collection, IEnumerable<T> input)
        {
            List<T> combination = new List<T>();
            combination.AddRange(collection);
            combination.AddRange(input);

            return combination;
        }

        /// <summary>
        /// Method combines the contents of this enumerable into a string separated by a given delimeter
        /// </summary>
        public static string ToString<T>(this IEnumerable<T> collection, string del = " ")
        {
            StringBuilder builder = collection.Count() > 0 ? new StringBuilder(collection.First().ToString()) : new StringBuilder();

            for (int i = 1; i < collection.Count(); i++)
            {
                builder.Append(del + collection.ElementAt(i).ToString());
            }

            return builder.ToString().Trim();
        }

        /// <summary>
        /// Method breaks this enumerable into differently sized arrays
        /// </summary>
        public static List<IEnumerable<T>> BreakIntoJaggedSubsets<T>(this IEnumerable<T> collection)
        {
            List<IEnumerable<T>> result = new List<IEnumerable<T>>();

            for (int i = 0; i < collection.Count(); i++)
            {
                T[] row = new T[collection.Count() - i];

                for (int j = 0; j < row.Count(); j++)
                {
                    row[j] = collection.ElementAt(i + j);
                }

                result.Add(row);
            }

            return result;
        }

        #endregion

        #region Filtering

        /// <summary>
        /// Method takes a subset of this enumerable given a start and end point
        /// </summary>
        public static IEnumerable<T> Subset<T>(this IEnumerable<T> collection, int start, int end)
        {
            int numberToTake = end - start;
            return collection.Skip(start).Take(numberToTake);
        }

        /// <summary>
        /// Method takes all but the last 'n' number of elements from this enumerable, given the value specified 
        /// </summary>
        public static IEnumerable<T> Less<T>(this IEnumerable<T> collection, int less)
        {
            return collection.Reverse().Skip(1).Reverse();
        }

        /// <summary>
        /// Method pulls a random element from this enumerable
        /// </summary>
        public static T GetRandomElement<T>(this IEnumerable<T> collection)
        {
            Random rand = new Random();
            int index = rand.Next(collection.Count());

            return collection.ElementAt(index);
        }

        /// <summary>
        /// Method extracts elements from this enumerable that are not contained in a given enumerable
        /// </summary>
        public static IEnumerable<T> WhereNotIn<T>(this IEnumerable<T> collection, IEnumerable<T> input)
        {
            return collection.Where(elem => !input.Contains(elem));
        }

        /// <summary>
        /// Method extracts elements from this enumerable that are not null
        /// </summary>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> collection)
        {
            return collection.Where(elem => elem != null);
        }

        /// <summary>
        /// Method extracts elements from this enumerable that are equal to a given value
        /// </summary>
        public static IEnumerable<T> WhereEqualTo<T>(this IEnumerable<T> collection, T value)
        {
            return collection.Where(elem => elem.Equals(value));
        }

        /// <summary>
        /// Method return the first element in this enumerable that is equal to a give value
        /// </summary>
        public static T FirstMatch<T>(this IEnumerable<T> collection, T obj)
        {
            return collection.FirstOrDefault(o => o.Equals(obj));
        }

        /// <summary>
        /// Method return the last element in this enumerable that is equal to a give value
        /// </summary>
        public static T LastMatch<T>(this IEnumerable<T> collection, T obj)
        {
            return collection.LastOrDefault(o => o.Equals(obj));
        }

        /// <summary>
        /// Method returns elements from every 'nth' position in this enumerable
        /// </summary>
        public static IEnumerable<T> GetFieldsInNthPositions<T>(this IEnumerable<T> collection, int x)
        {
            List<int> indexes = new List<int>() { 0 };
            indexes.AddRange(x.GenerateMultiples(collection.Count()));

            List<T> elements = new List<T>();
            indexes.ForEach(index => elements.Add(collection.ElementAt(index)));

            return elements;
        }

        /// <summary>
        /// Method takes the last 'n' elements from this enumerable
        /// </summary>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> collection, int number)
        {
            return collection.Skip(Math.Max(0, collection.Count() - number));
        }

        #endregion

        #region File / IO

        /// <summary>
        /// Method writes elements of this enumerable in text form out to a give file location
        /// </summary>
        public static void WriteToFile<T>(this IEnumerable<T> collection, string path)
        {
            string[] contents = collection
                .Select(elem => elem.ToString())
                .ToArray();

            File.WriteAllLines(path, contents);
        }

        /// <summary>
        /// Method writes elements of this enumerable out to a give file location based on a specified property of each element
        /// </summary>
        public static void WriteToFile<T>(this IEnumerable<T> collection, string path, Func<T, string> method)
        {
            string[] contents = collection
                .Select(elem => method(elem))
                .ToArray();

            File.WriteAllLines(path, contents);
        }

        /// <summary>
        /// Method writes element of this enumerable out to a file on the desktop
        /// </summary>
        public static void WriteToDesktop<T>(this IEnumerable<T> collection, string name)
        {
            collection.WriteToFile(Folders.MakePath(Folders.CommonFolders.Desktop, name));
        }

        #endregion

        #region Repeated Execution

        /// <summary>
        /// Method Executes a given action on each element of collection in parrallel (NOTE: Output is NON-DETERMINISTIC)
        /// </summary>
        public static void ExecuteInParallel<T>(this IEnumerable<T> collection, Action<T> action, int numberOfThreads = 8, bool speak = false)
        {
            ParallelUtility.ExecuteInParallel(collection, action, numberOfThreads, speak);
        }

        #endregion

        #endregion

        #region Strings

        #region Ordering

        /// <summary>
        /// Method orders this enumerable of strings alphabetically
        /// </summary>
        public static IEnumerable<string> OrderByAbc(this IEnumerable<string> collection)
        {
            return collection.OrderBy(elem => elem);
        }

        /// <summary>
        /// Method orders this enumerable of strings alphabetically, in reverse
        /// </summary>
        public static IEnumerable<string> OrderByAbcDescending(this IEnumerable<string> collection)
        {
            return collection.OrderByDescending(elem => elem);
        }

        /// <summary>
        /// Method orders this enumerable of strings by the length of each string
        /// </summary>
        public static IEnumerable<string> OrderByLength(this IEnumerable<string> collection)
        {
            return collection.OrderBy(elem => elem.Length);
        }

        /// <summary>
        /// Method orders this enumerable of strings by the length of each string, in reverse
        /// </summary>
        public static IEnumerable<string> OrderByLengthDescending(this IEnumerable<string> collection)
        {
            return collection.OrderByDescending(elem => elem.Length);
        }

        #endregion

        #region Alteration

        /// <summary>
        /// Method trims each string in this enumerable
        /// </summary>
        public static IEnumerable<string> TrimAll(this IEnumerable<string> collection)
        {
            return collection.Select(bit => bit.Trim());
        }

        /// <summary>
        /// Method makes each string in this enumerable lower case
        /// </summary>
        public static IEnumerable<string> AllLower(this IEnumerable<string> collection)
        {
            return collection.Select(element => element.ToLower());
        }

        /// <summary>
        /// Method makes each string in this enumerable upper case
        /// </summary>
        public static IEnumerable<string> AllUpper(this IEnumerable<string> collection)
        {
            return collection.Select(element => element.ToUpper());
        }

        /// <summary>
        /// Method joins each string in this enumerable on a given delimeter
        /// </summary>
        public static string JoinOnDelimeter(this IEnumerable<string> collection, string delim)
        {
            StringBuilder builder = new StringBuilder();

            if(collection.Count() > 0)
            {
                collection.Less(1).ForEach((elem) => builder.Append(elem + delim));
                builder.Append(collection.Last());
            }
  
            return builder.ToString().Trim();
        }

        #endregion

        #region Filtering

        /// <summary>
        /// Method returns strings from this enumerable for which no string in the given enumerable contains them
        /// </summary>
        public static IEnumerable<string> WhereNoneContain(this IEnumerable<string> collection, IEnumerable<string> input)
        {
            return collection.Where(elem => !input.Any(inp => elem.ContainsIgnoreCase(inp)));
        }

        /// <summary>
        /// Method returns strings from this enumerable that are not contained in the given string
        /// </summary>
        public static IEnumerable<string> WhereNotContained(this IEnumerable<string> collection, string value)
        {
            return collection.Where(elem => !elem.Contains(value));
        }

        /// <summary>
        /// Method returns strings from this enumerable for which no string in the given enumerable equals them
        /// </summary>
        public static IEnumerable<string> WhereNoneEqual(this IEnumerable<string> collection, IEnumerable<string> values)
        {
            return collection.Where(elem => !values.Any(val => elem.Equals(val)));
        }

        /// <summary>
        /// Method returns strings from this enumerable that are not equal to the given string
        /// </summary>
        public static IEnumerable<string> WhereNotEqual(this IEnumerable<string> collection, string value)
        {
            return collection.Where(elem => !elem.Equals(value));
        }

        /// <summary>
        /// Method returns strings from this enumerable that are not null or empty
        /// </summary>
        public static IEnumerable<string> WhereNotEmpty(this IEnumerable<string> collection)
        {
            return collection.Where(elem => !string.IsNullOrEmpty(elem));
        }

        /// <summary>
        /// Method returns strings from this enumerable that match a given regex pattern
        /// </summary>
        public static IEnumerable<string> WhereMatches(this IEnumerable<string> collection, Regex reg)
        {
            return collection.Where(elem => reg.IsMatch(elem));
        }

        /// <summary>
        /// Method returns from this enumerable the string closest to the given string
        /// </summary>
        public static KeyValuePair<string, double> GetClosestMatch(this IEnumerable<string> collection, string value)
        {
            var misses = collection.Select(elem => new { Text = elem, Miss = elem.HowCloseTo(value) })
                .OrderByDescending(val => val.Miss);

            var best = misses.First();

            return new KeyValuePair<string, double>(best.Text, best.Miss);
        }

        /// <summary>
        /// Method returns whether all strings in this enumerable ARE NOT null or empty
        /// </summary>
        public static bool AreNoneEmpty(this IEnumerable<string> collection)
        {
            return collection.All(elem => !string.IsNullOrEmpty(elem));
        }

        /// <summary>
        /// Method returns whether all strings in this enumerable ARE null or empty
        /// </summary>
        public static bool AreAllEmpty(this IEnumerable<string> collection)
        {
            return collection.All(elem => string.IsNullOrEmpty(elem));
        }

        #endregion

        #region Checking

        /// <summary>
        /// Method checks if any strings in this enumerable contain a given string
        /// </summary>
        public static bool AnyContain(this IEnumerable<string> collection, string str)
        {
            return collection.Any(elem => elem.ContainsIgnoreCase(str));
        }

        /// <summary>
        /// Method checks if any strings in this enumerable equal a given string
        /// </summary>
        public static bool AnyEqual(this IEnumerable<string> collection, string str)
        {
            return collection.Any(elem => elem.EqualsIgnoreCase(str));
        }

        #endregion

        #endregion

        #region Numerics

        #region Filtering

        /// <summary>
        /// Method returns numbers from this enumerable that are less than a given value
        /// </summary>
        public static IEnumerable<T> WhereLessThan<T>(this IEnumerable<T> collection, T value) where T : IComparable<T>
        {
            return collection.Where(element => element.CompareTo(value) < 0);
        }

        /// <summary>
        /// Method returns numbers from this enumerable that are greater than a given value
        /// </summary>
        public static IEnumerable<T> WhereGreaterThan<T>(this IEnumerable<T> collection, T value) where T : IComparable<T>
        {
            return collection.Where(element => element.CompareTo(value) > 0);
        }

        /// <summary>
        /// Method returns numbers from this enumerable that are between two given values
        /// </summary>
        public static IEnumerable<T> WhereBetween<T>(this IEnumerable<T> collection, T low, T high) where T : IComparable<T>
        {
            return collection.WhereLessThan(high).Intersect(collection.WhereGreaterThan(low));
        }

        #endregion

        #region Nature

        /// <summary>
        /// Method returns whether any numbers in this enumerable are prime
        /// </summary>
        public static bool AnyPrimes(this IEnumerable<int> numbers) { return numbers.Any(number => number.IsPrime()); }
        public static bool AnyPrimes(this IEnumerable<double> numbers){ return numbers.Any(number => number.IsPrime()); }

        /// <summary>
        /// Method returns whether any numbers in this enumerable are divisible by a given divider
        /// </summary>
        public static bool AllDivisibleBy(this IEnumerable<int> numbers, int divider) { return numbers.All(number => number % divider == 0); }
        public static bool AllDivisibleBy(this IEnumerable<double> numbers, double divider) { return numbers.All(number => number % divider == 0); }

        /// <summary>
        /// Method returns whether all numbers in this enumerable are either even or odd
        /// </summary>
        public static bool AllSameNature(this IEnumerable<int> numbers) { return numbers.AllToDouble().AllSameNature(); }
        public static bool AllSameNature(this IEnumerable<double> numbers)
        {
            if (numbers.Count() == 0) { return false; }
            return numbers.ElementAt(0).IsOdd() ? !numbers.Any(n => n.IsEven()) : !numbers.Any(n => n.IsOdd());
        }

        #endregion

        #region Arithmetic

        /// <summary>
        /// Method computes the median of the numbers in this enumerable
        /// </summary>
        public static int Median(this IEnumerable<int> numbers) { return numbers.AllToDouble().Median().ToInt(); }
        public static double Median(this IEnumerable<double> numbers)
        {
            var sorted = numbers.OrderBy(n => n);

            int count = sorted.Count();
            bool even = count % 2 == 0;

            int middle = (int)Math.Ceiling((double)(count / 2));

            if (even)
            {
                return sorted.ElementAt(middle - 1).Average(sorted.ElementAt(middle));
            }
            else
            {
                return Convert.ToDouble(sorted.ElementAt(middle));
            }
        }

        /// <summary>
        /// Method computes the standard deviation of the numbers in this enumerable
        /// </summary>
        public static double StandardDeviation(this IEnumerable<int> numbers) { return numbers.AllToDouble().StandardDeviation(); }
        public static double StandardDeviation(this IEnumerable<double> numbers)
        {
            double mean = numbers.Average();
            double sumOfSquares = numbers.Select(n => (n - mean).Square()).Sum();

            return Math.Sqrt(sumOfSquares / numbers.Count());
        }

        /// <summary>
        /// Method finds the Greatest Common Factor of the numbers in this enumerable
        /// </summary>
        public static int FindGCF(this IEnumerable<int> numbers) { return numbers.AllToDouble().FindGCF().ToInt(); }
        public static double FindGCF(this IEnumerable<double> numbers)
        {
            if (!numbers.AllSameNature() || numbers.AnyPrimes()) { return 1; }

            int minimum = numbers.Min().ToInt();

            for (int i = minimum; i > 0; i -= 2)
            {
                if (numbers.AllDivisibleBy(i))
                {
                    return i;
                }
            }

            return 1;
        }

        /// <summary>
        /// Method finds the Least Common Multiple of the numbers in this enumerable
        /// </summary>
        public static int FindLCM(this IEnumerable<int> numbers) { return numbers.AllToDouble().FindLCM().ToInt(); }
        public static double FindLCM(this IEnumerable<double> numbers)
        {
            double result = -1;

            if (numbers.Count() == 1)
            {
                result = numbers.ElementAt(0);
            }
            else
            {
                double last = numbers.ElementAt(numbers.Count() - 1);
                double second = numbers.ElementAt(numbers.Count() - 2);

                double prev = last.FindLCM(second);

                for (int i = numbers.Count() - 3; i >= 0; i--)
                {
                    prev = numbers.ElementAt(i).FindLCM(prev);
                }

                result = prev;
            }

            return result;
        }

        #endregion

        #region Manipulation

        /// <summary>
        /// Method converts all numbers in this enumerable to the double type
        /// </summary>
        public static IEnumerable<double> AllToDouble(this IEnumerable<int> collection) { return collection.Select(elem => elem.ToDouble()); }

        /// <summary>
        /// Method converts all numbers in this enumerable to the int type
        /// </summary>
        public static IEnumerable<int> AllToInt(this IEnumerable<double> collection) { return collection.Select(elem => elem.ToInt()); }

        /// <summary>
        /// Method filters outliers out of this enumerable recursively until none remain
        /// </summary>
        public static IEnumerable<double> FilterOutliers(this IEnumerable<double> collection)
        {
            var median = collection.Median();
            var sd = collection.StandardDeviation();
            var percentage = .3;

            var filtered = collection.Where(elem => !elem.IsOutlier(median, sd, percentage));

            return filtered.Count() == collection.Count() ? filtered : 
                                       filtered.Count() == 0 ? collection : filtered.FilterOutliers();
        }

        #endregion

        #endregion
    }
}
