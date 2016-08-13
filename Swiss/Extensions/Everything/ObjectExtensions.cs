using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Reflection;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace Swiss
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Method gets the type of this objec and return it casted to that type
        /// </summary>
        public static dynamic MakeOwnType(this object obj)
        {
            var type = obj.GetType();
            return Convert.ChangeType(obj, type);
        }

        /// <summary>
        /// Method serializes this object into a json string
        /// </summary>
        public static string ToJSON(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Method serializes this object into an xml string
        /// </summary>
        public static string ToXML(this object obj)
        {
            var type = obj.GetType();
            XmlSerializer serializer = new XmlSerializer(type);

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, obj);
                    return stringWriter.ToString();
                }
            }
        }

        /// <summary>
        /// Method serializes this object into a .bin file on the Desktop
        /// </summary>
        public static void SerializeToDesktop(this object obj, string nameOfFile)
        {
            if(obj.GetType().IsSerializable)
            {
                FileStream fs = new FileStream(Folders.Desktop + "/" + nameOfFile + ".bin", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);
                fs.Close();
            }
        }

        /// <summary>
        /// Method makes an ExpandoObject of this object
        /// </summary>
        public static dynamic MakeExpandoObject(this object obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            var props = TypeDescriptor.GetProperties(obj.GetType());

            foreach (PropertyDescriptor property in props)
                expando.Add(property.Name, property.GetValue(obj));

            return expando as ExpandoObject;
        }

        /// <summary>
        /// Method makes a new Stem of this object
        /// </summary>
        public static Stem MakeStem(this object obj)
        {
            //var exp = obj.MakeExpandoObject();
            return new Stem(obj);
        }

        /// <summary>
        /// Method gets a property of this object based on the name given
        /// </summary>
        public static dynamic GetProperty(this object obj, string fieldName)
        {
            return obj.GetType().GetProperty(fieldName).GetValue(obj, null).MakeOwnType();
        }

        /// <summary>
        /// Method gets a private property of this object based on the name given
        /// </summary>
        public static object GetPrivateProperty(this object obj, string fieldName)
        {
            var fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return fieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Method gets the class name of this object
        /// </summary>
        public static string GetNameOfClass(this object obj)
        {
            return obj.GetType().Name;
        }

        /// <summary>
        /// Method breaks properties of this object into their names and types
        /// </summary>
        public static Dictionary<string, Type> GetPropertyNamesAndTypes(this object obj)
        {
            Type incomingType = obj.GetType();
            Dictionary<string, Type> result = incomingType.GetPropertyNamesAndTypes();

            return result;
        }
    }
}
