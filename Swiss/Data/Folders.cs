using System;
using System.IO;

namespace Swiss
{
    /// <summary>
    /// Class holds data regarding special folders found on PC along with methods for generating folder paths
    /// </summary>
    public class Folders
    {
        public enum CommonFolders
        {
            Desktop,
            Downloads,
            Documents
        }

        public static string Profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public static string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 
        public static string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); 
        public static string Music = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); 
        public static string Pictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); 
        public static string Program64 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static string Program32 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86); 
        public static string Recents = Environment.GetFolderPath(Environment.SpecialFolder.Recent); 
        public static string Downloads = Path.Combine(Profile, "Downloads"); 

        public static string MakePath(CommonFolders folder, string extension)
        {
            string parent =
                folder.Equals(CommonFolders.Desktop) ? Desktop :
                folder.Equals(CommonFolders.Documents) ? Documents :
                Downloads;

            return Path.Combine(parent, extension);
        }
    }
}
