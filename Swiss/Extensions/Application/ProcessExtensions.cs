using System;
using System.Linq;
using System.Diagnostics;

namespace Swiss
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Method returns whether or not the process is running
        /// </summary>
        public static bool IsRunning(this Process p)
        {
            return Process.GetProcesses().Contains(p);
        }

        /// <summary>
        /// Method returns the exe path of a given process
        /// </summary>
        public static string GetEXE(this Process p)
        {
            string result = string.Empty;

            try
            {
                result = p.Modules[0].FileName;
            }
            catch (Exception trouble) { }

            return result;
        }
    }
}
