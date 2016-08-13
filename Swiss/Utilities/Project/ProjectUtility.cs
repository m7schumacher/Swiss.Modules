using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Swiss
{
    /// <summary>
    /// Utility class for gathering contents, metrics, and other data from a .csproj or solution
    /// </summary>
    public class ProjectUtility
    {
        private static IEnumerable<string> FindIncludedCSFiles(IEnumerable<string> projs)
        {
            string xmlnamespace = "{http://schemas.microsoft.com/developer/msbuild/2003}";

            return from proj in projs
                   let xml = XDocument.Load(proj)
                   let dir = Path.GetDirectoryName(proj)
                   from c in xml.Descendants(xmlnamespace + "Compile")
                   let inc = c.Attribute("Include").Value
                   where inc.EndsWith(".cs")
                   select Path.Combine(dir, c.Attribute("Include").Value);
        }

        public static IEnumerable<string> FindOrphanedFiles(string source)
        {
            var allFiles = GetAllCSFiles(source);
            var csprojs = GetCSProjs(source);
            var includedFiles = FindIncludedCSFiles(csprojs);

            return allFiles.Except(includedFiles);
        }

        public static IEnumerable<string> GetAllCSFiles(string source)
        {
            return Directory.EnumerateFiles(source, "*.cs", SearchOption.AllDirectories)
                .Where(p => !p.Contains(@"\obj\"))
                .Where(p => !p.Contains(@"\bin\"))
                .ToList();
        }

        public static IEnumerable<string> GetCSProjs(string source)
        {
            return Directory.EnumerateDirectories(source, "*.csproj", SearchOption.AllDirectories);
        }
    }
}
