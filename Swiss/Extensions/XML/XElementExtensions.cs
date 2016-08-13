using System.Collections.Generic;
using System.Xml.Linq;

namespace Swiss
{
    public static class XElementExtensions
    {
        /// <summary>
        /// Method returns whether this XElement has a given attribute
        /// </summary>
        public static bool HasAttribute(this XElement elem, string attribute)
        {
            return elem.Attribute(attribute) != null;
        }

        /// <summary>
        /// Method returns all descendants of this XElement
        /// </summary>
        public static List<XElement> GetAllDescendants(this XElement element)
        {
            List<XElement> elements = new List<XElement>();

            foreach (XElement child in element.Elements())
            {
                elements.Add(child);
                elements.AddRange(GetAllDescendants(child));
            }

            return elements;
        }
    }
}
