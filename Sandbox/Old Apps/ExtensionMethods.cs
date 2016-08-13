using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Old_Apps
{
    public static class Extensions
    {
        public static IEnumerable<T> AlterIf<T>(this IEnumerable<T> collection, Func<T, bool> filter, Func<T, T> mutator)
        {
            return collection.Select(x => filter(x) ? mutator(x) : x);
        }

        public static void AddIf<T>(this ICollection<T> collection, Func<T, bool> function, T value)
        {
            if (function(value))
            {
                collection.Add(value);
            }
        }

        public static void AddIfNotContains<T>(this ICollection<T> collection, T value)
        {
            collection.AddIf(x => !collection.Contains(value), value);
        }

        public static void AddRangeIfNotContains<T>(this ICollection<T> collection, IEnumerable<T> parts)
        {
            foreach (T part in parts)
            {
                collection.AddIfNotContains(part);
            }
        }

        public static IEnumerable<V> TranslateTo<T, V>(this IEnumerable<T> collection, Func<T, V> func)
        {
            return collection.Select(x => func(x));
        }
    }

    public class ExtensionMethods : Project
    {
        public override void Execute()
        {
            string[] myArray = new string[] { "1", "2", "3" };

            List<int> intValues = myArray.TranslateTo(x => Convert.ToInt32(x)).ToList();

            intValues.AddIf(x => !intValues.Contains(5), 5);
            intValues.AddIfNotContains(6);
            intValues.AddIfNotContains(3);
            intValues.AddRangeIfNotContains(new int[] { 1, 5, 7, 8, 4 });

            intValues = intValues.AlterIf(x => x == 5, x => x + 2).ToList();
        }
    }
}
