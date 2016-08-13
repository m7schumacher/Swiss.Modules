using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Swiss
{
    public class SystemUtility
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        private static PerformanceCounter _counter;

        public static double GetRamUsage(Process p)
        {
            _counter = new PerformanceCounter("Process", "Working Set", p.ProcessName);
            double value = _counter.NextValue() / 1024 / 1024;
            _counter.Close();

            return value.ToInt();
        }

        public static double GetCpuUsage()
        {
            PerformanceCounter local = new PerformanceCounter();

            local.CategoryName = "Processor";
            local.CounterName = "% Processor Time";
            local.InstanceName = "_Total";

            var firstReading = local.NextValue();
            Thread.Sleep(1000);

            var usage = (double)local.NextValue();
            return usage.Round(2);
        }

        public static Dictionary<string, double> MonitorCPU(int numberOfReadings)
        {
            var CpuUsages = new Dictionary<string, double>();
            int readings = 0;

            while (readings < numberOfReadings)
            {
                CpuUsages.Add(DateTime.Now.ToLongTimeString(), GetCpuUsage());
            }

            return CpuUsages;
        }

        public static double GetCpuTime(Process p)
        {
            _counter = new PerformanceCounter("Process", "% Processor Time", p.ProcessName);
            return _counter.NextValue();
        }

        public static double GetBatteryPercentage()
        {
            PowerStatus pw = SystemInformation.PowerStatus;
            return pw.BatteryLifePercent;
        }

        public static double GetBatterLife()
        {
            PowerStatus pw = SystemInformation.PowerStatus;
            return pw.BatteryLifeRemaining;
        }

        public static double GetInternetSpeedInKB()
        {
            try
            {
                double[] speeds = new double[5];

                for (int i = 0; i < 5; i++)
                {
                    WebClient client = new WebClient();
                    DateTime startTime = DateTime.Now;
                    client.DownloadData("http://www.google.com");
                    DateTime endTime = DateTime.Now;
                    speeds[i] = Math.Round((53 / (endTime - startTime).TotalSeconds), 2);
                }

                return speeds.Average();
            }
            catch (Exception trouble)
            {
                return -1;
            }
        }
    }
}
