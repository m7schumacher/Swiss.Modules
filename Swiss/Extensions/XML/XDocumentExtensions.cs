using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Xml;

namespace Swiss
{
    public static class XDocumentExtensions
    {
        /// <summary>
        /// Method converts this XmlDocument to an XDocument
        /// </summary>
        public static XDocument ToXDocument(this XmlDocument doc)
        {
            using (var nodeReader = new XmlNodeReader(doc))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        /// <summary>
        /// Method returns every element found in this XDocument
        /// </summary>
        public static List<XElement> GetAllElements(this XDocument doc)
        {
            List<XElement> elements = new List<XElement>();

            foreach(XElement child in doc.Elements())
            {
                elements.Add(child);
                elements.AddRange(child.GetAllDescendants());
            }

            return elements;
        }

        /// <summary>
        /// Method wraps this XDocument in an XMLSheet
        /// </summary>
        public static XmlSheet ToXmlSheet(this XDocument doc)
        {
            return new XmlSheet(doc);
        }

        /// <summary>
        /// Method returns elements in this XDocument with a given name
        /// </summary>
        public static List<XElement> GetElementsByName(this XDocument doc, string name)
        {
            return doc.GetAllElements()
                .Where(elem => elem.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Method returns elements in this XDocument with a given attribute
        /// </summary>
        public static List<XElement> GetElementsWithAttribute(this XDocument doc, string attribute)
        {
            return doc.GetAllElements()
                .Where(elem => elem.Attribute(attribute) != null)
                .ToList();
        }

        /// <summary>
        /// Method returns elements in this XDocument with a given name and attribute
        /// </summary>
        public static List<XElement> GetElementsByNameWithAttribute(this XDocument doc, string name, string attribute)
        {
            return doc.GetElementsByName(name)
                .Where(elem => elem.Attribute(attribute) != null)
                .ToList();
        }

        /// <summary>
        /// Method returns elements in this XDocument with a given attribute value
        /// </summary>
        public static List<XElement> GetElementsByAttributeValue(this XDocument doc, string attribute, string value)
        {
            var elementsWithAttribute = doc.GetElementsWithAttribute(attribute);

            return elementsWithAttribute
                .Where(elem => elem.Attribute(attribute).Equals(value))
                .ToList();
        }

        /// <summary>
        /// Method serializes this XDocument into JSON
        /// </summary>
        public static string ConvertToJSON(this XDocument doc)
        {
            return JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.None, false);
        }

        /// <summary>
        /// Method serializes this XmlDocument into JSON
        /// </summary>
        public static string ConvertToJSON(this XmlDocument doc)
        {
            return JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, false);
        }
    }
}
