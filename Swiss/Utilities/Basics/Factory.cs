using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Swiss
{
    #region Domain

    /// <summary>
    /// Wrapper class around the ExpandoObject
    /// </summary>
    public class Stem
    {
        private object _core;

        public Type Type { get { return _core.GetType(); } }

        public string[] Properties { get { return Type.GetProperties().Select(prop => prop.Name).ToArray(); } }
        public Tuple<dynamic>[] Values { get { return Properties.Select(prop => new Tuple<dynamic>(_core.GetProperty(prop))).ToArray(); } }

        public Stem(object obj)
        {
            _core = obj;
        }

        public dynamic Prop(string property)
        {
            return _core.GetProperty(property);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < Properties.Length; i++)
            {
                builder.Append(String.Format("{0}: {1}, ", Properties[i], Values[i].ToString()));
            }

            return builder.ToString();
        }
    }

    /// <summary>
    /// Wrapper class around a dynamically created type (class)
    /// </summary>
    public class Foundation
    {
        private object _foundation { get; set; }

        public string Text { get; set; }
        public string Name { get; set; }

        public List<Stem> Members { get; set; }

        public Foundation(object obj, string text, string name)
        {
            _foundation = obj;
            Text = text;
            Name = name;
        }

        public Foundation(object obj, string text, string name, List<Stem> members) : this(obj, text, name)
        {
            Members = members;
        }

        public void AddMember(object[] values)
        {
            Stem stem = Factory.GenerateStem(values);
            Members.Add(stem);
        }

        public dynamic Exec(string name, params object[] parameters)
        {
            return _foundation.GetType().GetMethod(name).Invoke(_foundation, parameters);
        }

        public void MakeFile(string location)
        {
            var path = location + "/" + Name + ".cs";
            File.WriteAllText(path, Text);
        }
    }

    #endregion

    /// <summary>
    /// Utility class can be used to generate dynamic objects, or to generate .cs files for a given object
    /// </summary>
    public class Factory
    {
        #region Stem and Object Generation

        public static Tuple<T> GenerateTuple<T>(T value)
        {
            return new Tuple<T>(value);
        }

        /// <summary>
        /// Generates a Stem object based on types and values of a given set of objects
        /// </summary>
        public static Stem GenerateStem(params object[] values)
        {
            dynamic obj = new ExpandoObject();
            var dict = obj as IDictionary<string, object>;

            for(int i = 0; i < values.Length; i++)
            {
                var actual = values[i].MakeOwnType();
                dict["prop" + i] = actual;
            }

            return new Stem(obj);
        }

        /// <summary>
        /// Generates a Stem object given the names, types, and values of a table. Useful when scraping HTML tables
        /// </summary>
        public static List<Stem> GenerateStemsFromGrid(string[] header, string[][] grid)
        {
            List<Stem> stems = new List<Stem>();

            string[] attributes = header;

            for (int i = 0; i < grid.Length; i++)
            {
                dynamic obj = new ExpandoObject();
                var dict = obj as IDictionary<string, object>;

                string[] row = grid[i];

                for (int j = 0; j < grid[i].Length; j++)
                {
                    var literal = row[j];
                    var name = attributes[j];

                    double numericValue;

                    if (double.TryParse(literal, out numericValue))
                    {
                        dict[name] = numericValue;
                    }
                    else
                    {
                        dict[name] = literal;
                    }
                }

                stems.Add(new Stem(obj));
            }

            return stems;
        }

        #endregion

        #region Types - .CS file generation

        #region General .CS Generation

        /// <summary>
        /// Method generates .CS file for a given object, compiles the generated class, and wraps it in a Foundation
        /// </summary>
        public static Foundation LayFoundation(object obj, string name, string space)
        {
            string text = GenerateCS(obj, name, space);
            Foundation foundation = TurnTextIntoClass(text, name, space);

            return foundation;
        }

        /// <summary>
        /// Method generates the content of a .CS file from the property types and values of a given object
        /// </summary>
        public static string GenerateCS(object obj, string name, string space)
        {
            Stem stem = obj.MakeStem();

            var props = new Dictionary<string, string>();

            for (int i = 0; i < stem.Properties.Count(); i++)
            {
                var property = RefinePropertyName(stem.Properties[i]);

                if (props.ContainsKey(property))
                {
                    continue;
                }

                var value = stem.Values.ElementAt(i);
                var type = value.Item1.GetType().ToString().Split('.')[1];
                type = RefinePropertyType(type);

                props.Add(property, type);
            }

            var classText = WriteClass(props, name);
            return WriteFile(classText, space);
        }

        /// <summary>
        /// Method generates the content of a .CS file from the property types and values of a given ExpandoObject (Dictionary of strings)
        /// </summary>
        public static string GenerateCS(ExpandoObject obj, string name)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            foreach (var pair in obj)
            {
                var nm = RefinePropertyName(pair.Key);
                var tp = pair.Value.GetType().ToString().Split('.')[1];
                var type = RefinePropertyType(tp);

                var value = pair.Value.ToString();

                if (value.StartsWith("List:"))
                {
                    type = value;
                }

                props.Add(nm, type);
            }

            return WriteClass(props, name);
        }

        /// <summary>
        /// Method refines a given property into a format suitable for a .CS file
        /// </summary>
        private static string RefinePropertyName(string property)
        {
            property = HttpUtility.UrlDecode(property);

            var invalids = new string[]
            {
                "nbsp"
            };

            property = property.RemoveAll(invalids);

            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                { "0", "Zero" },
                { "1", "One" },
                { "2", "Two" },
                { "3", "Three" },
                { "4", "Four" },
                { "5", "Five" },
                { "6", "Six" },
                { "7", "Seven" },
                { "8", "Eight" },
                { "9", "Nine" }
            };

            values.ForEach(val => property = property.Replace(val.Key, val.Value));
            property = property.TrimPunctuation().RemoveSpaces();

            return property;
        }

        /// <summary>
        /// Method converts the type of a given property to a proper type suitable for a .CS file
        /// </summary>
        private static string RefinePropertyType(string type)
        {
            var primitiveTypes = new string[]
            {
                "Double", "String", "Float", "Decimal", "Boolean", "Dynamic"
            };

            if (primitiveTypes.Contains(type))
            {
                type = type.ToLower();
            }
            else if (type.Equals("Integer"))
            {
                type = "int";
            }

            return type;
        }

        /// <summary>
        /// Method compiles the text content of a given .CS file into a Foundation object
        /// </summary>
        private static Foundation TurnTextIntoClass(string text, string nameOfClass, string space)
        {
            object instance = null;
            string name = space + "." + nameOfClass;

            using (CSharpCodeProvider compiler = new CSharpCodeProvider())
            {
                var parameters = new CompilerParameters() { GenerateInMemory = true };
                var assembly = compiler.CompileAssemblyFromSource(parameters, text);
                var type = assembly.CompiledAssembly.GetType(name);

                instance = Activator.CreateInstance(type);
            }

            return new Foundation(instance, text, nameOfClass);
        }

        #endregion

        #region Grid Based .CS Generation

        /// <summary>
        /// Method generates a .cs file based on the types and values of a given table, class name, and namespace
        /// </summary>
        public static string GenerateCS(string[] header, string[][] grid, string nameOfClass, string namespaceOfClass)
        {
            List<Stem> stems = GenerateStemsFromGrid(header, grid);
            return GenerateCS(stems.First(), nameOfClass, namespaceOfClass);
        }

        #endregion

        #region XML Based .CS Generation

        /// <summary>
        /// Method generates a .cs file from the attributes and children found in a given XML node
        /// </summary>
        public static string GenerateCS(XmlNode node)
        {
            var exp = ParseNode(node);
            return GenerateCS(exp, node.Name);
        }

        /// <summary>
        /// Method parses an XML node into its attributes
        /// </summary>
        private static ExpandoObject ParseNode(XmlNode node)
        {
            IDictionary<string, object> localValues = new ExpandoObject();
            node.Attributes.ForEach(pair => localValues.Add(pair.Key, pair.Value));

            return localValues as ExpandoObject;
        }

        /// <summary>
        /// Method generates a .cs file from an XML sheet
        /// </summary>
        public static string GenerateCS(XmlSheet sheet, string namespaceOfClass)
        {
            var texts = new List<string>();

            var groups = sheet.Nodes.GroupBy(nd => nd.ToString());
            var leaves = new List<XmlNode>();
            var branches = new List<XmlNode>();

            foreach(var group in groups)
            {
                var target = group.OrderByDescending(nd => nd.Children.Count).First();

                if (target.IsLeaf) leaves.Add(target);
                else branches.Add(target);
            }

            foreach (var leaf in leaves)
            {
                var exp = ParseNode(leaf);
                var text = GenerateCS(exp, leaf.Name.ToString());
                texts.Add(text);
            }

            foreach (var branch in branches)
            {
                var nameOfBranch = branch.Name.ToString();

                if (branch.IsCollection)
                {
                    continue;
                }

                IDictionary<string, object> vals = ParseNode(branch);

                foreach (var child in branch.Children)
                {
                    var nameOfChild = child.Name;

                    if (child.IsCollection)
                    {
                        vals.Add(nameOfChild, "List:" + child.Children.First().Name);
                    }
                    else
                    {
                        vals.Add(nameOfChild, nameOfChild);
                    }
                }

                var exp = vals as ExpandoObject;
                var text = GenerateCS(exp, nameOfBranch);
                texts.Add(text);
            }

            StringBuilder builder = new StringBuilder();

            foreach (string text in texts)
            {
                builder.AppendLine(text);
            }

            string file = WriteFile(builder.ToString(), "Swiss.Sanford");

            return file;
        }

        #endregion

        #region .CS Writing

        /// <summary>
        /// Method generates the content of a .CS file given a list of properties and their types along with a class name
        /// </summary>
        private static string WriteClass(Dictionary<string, string> props, string name)
        {
            if (props == null)
            {
                props = new Dictionary<string, string>();
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("\tpublic class " + name + "\n\t{");

            foreach (var pair in props)
            {
                var value = pair.Value;

                if (value.StartsWith("List:"))
                {
                    var child = value.Split(":")[1];
                    builder.AppendLine("\t\tpublic List<" + child + "> " + pair.Key + " { get; set; }");
                }
                else
                {
                    builder.AppendLine("\t\tpublic " + pair.Value + " " + pair.Key + " { get; set; }");
                }
            }

            builder.AppendLine();
            builder.AppendLine("\t\tpublic " + name + "()\n\t\t{\n");
            builder.AppendLine("\t\t}");

            builder.AppendLine();
            builder.AppendLine("\t\tpublic " + name + "(object[] values)\n\t\t{");

            for (int i = 0; i < props.Count; i++)
            {
                var pair = props.ElementAt(i);

                var value = pair.Value.StartsWith("List:") ? "List<" + pair.Value.Split(":")[1] + ">" : pair.Value;
                builder.AppendLine("\t\t\t" + pair.Key + " = " + "!String.IsNullOrEmpty(values[" + i + "].ToString()) ? (" + value + ")values[" + i + "] : default(" + value + ");");
            }

            builder.AppendLine("\t\t}");
            builder.AppendLine("\t}");

            return builder.ToString();
        }

        /// <summary>
        /// Method appens default using statements and namespace declarations to the content of a .CS file
        /// </summary>
        private static string WriteFile(string body, string space)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("using System;\nusing System.Collections.Generic;\n" +
                "using System.Text;\nusing System.Threading.Tasks;\nusing Swiss;\n\n");

            builder.AppendLine("namespace " + space + "\n{");
            builder.Append(body);
            builder.AppendLine("}");

            return builder.ToString();
        }

        /// <summary>
        /// Method writes out the content of a .CS file to an actual .CS at the location specified
        /// </summary>
        public static void GenerateFile(string text, string name, string location)
        {
            var path = location + "/" + name + ".cs";
            File.WriteAllText(path, text);
        }

        #endregion

        #endregion
    }
}
