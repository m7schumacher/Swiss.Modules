using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Swiss
{
    /// <summary>
    /// Utility class for working with an analyzing running processes
    /// </summary>
    public class ProcessUtility
    {
        /// <summary>
        /// Method returns a process object given the name of a RUNNING process
        /// </summary>
        public static Process GetProcess(string exe)
        {
            Process proc = null;

            try
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.Modules[0].FileName.Equals(exe))
                    {
                        proc = p;
                        break;
                    }
                }
            }
            catch (Exception trouble) { }

            return proc;
        }

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);

        /// <summary>
        /// Method brings a given process to the front of the screen
        /// </summary>
        public static void BringProcessToFront(Process p)
        {
            IntPtr handle = p.MainWindowHandle;

            if (IsIconic(handle))
            {
                ShowWindow(handle, 9);
            }
            else
            {
                SetForegroundWindow(handle);
            }
        }

        /// <summary>
        /// Method checks if a given process name is running
        /// </summary>
        public static bool IsProcessRunning(string processName)
        {
            Process p = null;
            processName = processName.ToLower();
            string exe = string.Empty;

            try
            {
                p = Process.GetProcesses().FirstOrDefault(process => process.GetEXE().ToLower().Equals(processName));
            }
            catch (Exception trouble) { }

            if (p != null)
            {
                BringProcessToFront(p);
                return true;
            }

            return false;
        } 
    }
}
