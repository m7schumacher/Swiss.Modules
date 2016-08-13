using System;
using System.Linq;
using System.IO;

namespace Swiss
{
    public static class DirectoryExtensions
    {
        #region Deletion

        /// <summary>
        /// Method clears the contents of this directory
        /// </summary>
        public static void ClearContents(this DirectoryInfo dir)
        {
            dir.EnumerateFiles().ToList().ForEach(file => file.Delete());
            dir.EnumerateDirectories().ToList().ForEach(sub => sub.Delete(true));
        }

        /// <summary>
        /// Method deletes files in this directory that satisfy a condition
        /// </summary>
        public static void DeleteFilesWhere(this DirectoryInfo dir, Func<FileInfo, bool> predicate)
        {
            dir.GetFiles()
                .Where(file => predicate(file))
                .ToList()
                .ForEach(file => File.Delete(file.FullName));
        }

        /// <summary>
        /// Method deletes directories in this directory that satisfy a condition
        /// </summary>
        public static void DeleteDirectoriesWhere(this DirectoryInfo dir, Func<DirectoryInfo, bool> predicate)
        {
            dir.GetDirectories()
                .Where(sub => predicate(sub))
                .ToList()
                .ForEach(sub => Directory.Delete(sub.FullName, true));
        }

        /// <summary>
        /// Method deletes directories in this directory that have not been accessed since a given date
        /// </summary>
        public static void DeleteDirectoriesOlderThan(this DirectoryInfo dir, DateTime time)
        {
            dir.DeleteDirectoriesWhere(sub => sub.LastAccessTimeUtc < time.ToUniversalTime());
        }

        /// <summary>
        /// Method deletes files in this directory that have not been accessed since a given date
        /// </summary>
        public static void DeleteFilesOlderThan(this DirectoryInfo dir, DateTime time)
        {
            dir.DeleteFilesWhere(file => file.LastAccessTimeUtc < time.ToUniversalTime());
        }

        #endregion

        #region Metrics

        /// <summary>
        /// Method gets the size of this directory in bytes
        /// </summary>
        public static long GetSizeInBytes(this DirectoryInfo dir)
        {
            long sizeOfDirectory = 0;

            foreach (FileInfo fi in dir.GetFiles())
            {
                sizeOfDirectory += fi.Length;
            }

            foreach (DirectoryInfo sub in dir.GetDirectories())
            {
                sizeOfDirectory += sub.GetSizeInBytes();
            }

            return sizeOfDirectory;
        }

        /// <summary>
        /// Method returns the biggest file in this directory
        /// </summary>
        public static FileInfo GetLargestFile(this DirectoryInfo dir)
        {
            return dir.GetFiles().OrderBy(fl => fl.Length).Last();
        }

        /// <summary>
        /// Method returns the largest sub-folder in this directory
        /// </summary>
        public static DirectoryInfo GetLargestSubDirectory(this DirectoryInfo dir)
        {
            return dir.GetDirectories().OrderBy(sub => sub.GetSizeInBytes()).Last();
        }

        #endregion

        /// <summary>
        /// Method moves this directory to a given, already existing directory
        /// </summary>
        public static void MoveToExisting(this DirectoryInfo source, string destinationFolder)
        {
            string sourceFolder = source.FullName;

            if (Directory.Exists(destinationFolder))
            {
                var files = Directory.GetFiles(sourceFolder);

                foreach (string file in files)
                {
                    File.Copy(file, file.Replace(sourceFolder, destinationFolder), true);
                }

                //Directory.Delete(sourceFolder, true);
            }
            else
            {
                Directory.Move(sourceFolder, destinationFolder);
            }
        }
    }
}
