using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Swiss
{
    /// <summary>
    /// Class wraps the results of repeated runs of a given method or function
    /// </summary>
    public class PerformanceResult
    {
        public double AverageTime { get; set; }
        public double MedianTime { get; set; }
        public double StandardDevation { get; set; }

        public double BestTime { get; set; }
        public double WorstTime { get; set; }

        public List<double> Times { get; set; }

        public bool Accurate { get { return StandardDevation / AverageTime < .2; } }

        public PerformanceResult(List<double> times)
        {
            times = times.FilterOutliers().ToList();

            AverageTime = times.Average().Round(2);
            MedianTime = times.Median().Round(2);
            StandardDevation = times.StandardDeviation().Round(2);
            BestTime = times.Min().Round(2);
            WorstTime = times.Max().Round(2);
            Times = times;
        }
    }

    public struct RaceResult
    {
        public PerformanceResult LeftResult { get; set; }
        public PerformanceResult RightResult { get; set; }

        public string Message { get; set; }
    }

    /// <summary>
    /// Utility class with various tools to analyze performance of functions and get basic machine metrics
    /// </summary>
    public class DiagnosticUtility
    {
        #region Function Performance Evaluation and Comparison

        /// <summary>
        /// Method analyzes and returns the performance of a give function after having been run a given number of times
        /// </summary>
        public static PerformanceResult GetFunctionPerformance(Action action, int iterations)
        {
            List<double> times = new List<double>();
            var localTime = 0.0;

            while (times.Count != iterations)
            {
                IEnumerable<int> tries = Enumerable.Range(0, iterations - times.Count);

                tries.ForEach(tr =>
                {
                    localTime = TimeAction(action);
                    Console.Write(localTime + " ");
                    times.Add(localTime);
                });

                Console.WriteLine();
                times = times.FilterOutliers().ToList();//outliers are filtered out in order to collect an accurate sample
            }

            return new PerformanceResult(times);
        }

        /// <summary>
        /// Method compares the performance of two functions, determining which is fastest
        /// </summary>
        public static RaceResult DetermineFastestFunction(Action left, Action right, int iterations)
        {
            List<double> leftTimes = new List<double>();
            List<double> rightTimes = new List<double>();

            Task[] tasks = new Task[2];

            tasks[0] = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    leftTimes.Add(TimeAction(left));
                }
            });

            tasks[1] = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    rightTimes.Add(TimeAction(right));
                }
            });

            Task.WaitAll(tasks);

            var leftResult = new PerformanceResult(leftTimes);
            var rightResult = new PerformanceResult(rightTimes);

            var leftAverage = leftResult.AverageTime;
            var rightAverage = rightResult.AverageTime;

            var fastest = leftAverage < rightAverage ? "first" : "second";
            var slowest = leftAverage > rightAverage ? "first" : "second";

            var percentageFaster = Math.Round((Math.Abs(leftAverage - rightAverage) / Math.Max(leftAverage, rightAverage) * 100), 2);

            return new RaceResult()
            {
                LeftResult = leftResult,
                RightResult = rightResult,
                Message = string.Format("The {0} function was {1}% faster than the {2} function", fastest, percentageFaster, slowest)
            };
        }

        /// <summary>
        /// Method measures how long it takes a given action to run
        /// </summary>
        public static double TimeAction(Action act)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            act.Invoke();

            return watch.ElapsedMilliseconds;
        }

        #endregion

        #region System Hardware Metrics

        

        #endregion
    }
}
