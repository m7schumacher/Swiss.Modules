using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Swiss.Application
{
    public class ComputerUtility
    {
        //For Chrome
        private const int WM_GETTEXTLENGTH = 0Xe;
        private const int WM_GETTEXT = 0Xd;

        [DllImport("user32.dll")]
        private extern static int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private extern static int SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private extern static IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        public static string getChromeUrl(IntPtr winHandle)
        {
            string browserUrl = null;
            IntPtr urlHandle = FindWindowEx(winHandle, IntPtr.Zero, "Chrome_AutocompleteEditView", null);
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            int length = SendMessage(urlHandle, WM_GETTEXTLENGTH, 0, 0);
            if (length > 0)
            {
                SendMessage(urlHandle, WM_GETTEXT, nChars, Buff);
                browserUrl = Buff.ToString();

                return browserUrl;
            }
            else
            {
                return browserUrl;
            }

        }

        public static IntPtr GetRunningChrome()
        {
            IntPtr ChromeHandle = default(IntPtr);
            
            foreach (Process pro in Process.GetProcessesByName("chrome"))
            {
                ChromeHandle = pro.MainWindowHandle;

                if (!ChromeHandle.Equals(IntPtr.Zero))
                {
                    break;
                }
            }

            return ChromeHandle;
        }
    }
}
