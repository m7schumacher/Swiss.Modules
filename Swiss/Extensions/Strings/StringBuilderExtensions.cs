using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Swiss
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Method appends a set apart header line to this builder
        /// </summary>
        public static void AppendHeader(this StringBuilder builder, string header)
        {
            builder.AppendLine();
            builder.AppendLine(header);
            builder.AppendLine();
        }

        /// <summary>
        /// Method appends a physical line break composed of a given character to this builder
        /// </summary>
        public static void AppendBreak(this StringBuilder builder, char c = '-')
        {
            string line = string.Empty;

            Enumerable.Range(0, 40).ForEach((number) =>
            {
                line += c;
            });

            builder.AppendHeader(line);
        }

        /// <summary>
        /// Method appends the string version of each element in a given enumerable to this builder
        /// </summary>
        public static void AppendEach<T>(this StringBuilder builder, IEnumerable<T> collection)
        {
            collection.ForEach(val => builder.AppendLine(val.ToString()));
        }

        /// <summary>
        /// Method appends the value of a given property for each element in a given enumerable to this builder
        /// </summary>
        public static void AppendEach<T>(this StringBuilder builder, IEnumerable<T> collection, Func<T, string> method)
        {
            collection.ForEach(val => builder.AppendLine(method(val)));
        }

        /// <summary>
        /// Method appends the string version of each key value pair in a given Dictionary to this builder
        /// </summary>
        public static void AppendDictionary<T,K>(this StringBuilder builder, IDictionary<T,K> dictionary)
        {
            dictionary.Keys.ForEach(key => builder.AppendLine(key.ToString() + " -- " + dictionary[key].ToString()));
        }

        /// <summary>
        /// Method appends the product of each key value pair and a given function in a given Dictionary to this builder
        /// </summary>
        public static void AppendDictionary<T, K>(this StringBuilder builder, IDictionary<T, K> dictionary, Func<KeyValuePair<T,K>, string> mutator)
        {
            dictionary.ForEach(pair => builder.AppendLine(mutator(pair)));
        }

        /// <summary>
        /// Method appends the string version of each key value pair in a given Dictionary to this builder
        /// </summary>
        public static void AppendDictionary<T,K,V>(this StringBuilder builder, IDictionary<T,K> dictionary) where K : IEnumerable<V>
        {
            dictionary.Keys.ForEach(key =>
            {
                builder.AppendLine(key.ToString());
                builder.AppendLine();

                dictionary[key].ForEach(val => builder.AppendLine(val.ToString()));

                builder.AppendLine();
            });
        }

        /// <summary>
        /// Method writes the content of this builder out to a file
        /// </summary>
        public static void WriteToFile(this StringBuilder builder, string path)
        {
            File.WriteAllText(path, builder.ToString());
        }

        /// <summary>
        /// Method writes the content of this builder out to a file on the desktop
        /// </summary>
        public static void WriteToDesktop(this StringBuilder builder, string name)
        {
            File.WriteAllText(Folders.MakePath(Folders.CommonFolders.Desktop, name), builder.ToString());
        }
    }
}
