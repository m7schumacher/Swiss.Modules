using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swiss
{
    /// <summary>
    /// Utility class for multi-threading and other repeated execution
    /// </summary>
    public static class ParallelUtility
    {
        public static void DoMany(int times, Action act)
        {
            for (int i = 0; i < times; i++)
                act.Invoke();
        }

        /// <summary>
        /// Method Executes a given action on a collection in parrallel (NOTE: Output is Non-Deterministic)
        /// Essentially does the same thing as Parallel.ForEach
        /// </summary>
        public static void ExecuteInParallel<T>(IEnumerable<T> elements, Action<T> operation, int numberOfThreads = 8, bool speak = true)
        {
            int numberOfElements = elements.Count();

            if(numberOfElements > 0)
            {
                numberOfThreads = Math.Min(numberOfThreads, numberOfElements);

                int elementsPerThread = numberOfElements / numberOfThreads;

                List<Task> threads = new List<Task>();

                for (int i = 0; i < numberOfThreads; i++)
                {
                    int tempI = i;
                    threads.Add(Task.Factory.StartNew(() =>
                    {
                        int start = tempI * elementsPerThread;
                        int end = tempI == numberOfThreads - 1 ? numberOfElements : start + elementsPerThread;

                        if (speak)
                        {
                            Console.WriteLine("Thread {0} operating on {1} objects...", tempI, end - start);
                        }

                        for (int j = start; j < end; j++)
                        {
                            operation(elements.ElementAt(j));
                        }

                        if (speak)
                        {
                            Console.WriteLine("Thread {0} complete.", tempI);
                        }
                    }));
                }

                Task.WaitAll(threads.ToArray());
            } 
        }

        /// <summary>
        /// Method Executes a given function on a collection in parrallel (NOTE: Output is Non-Deterministic)
        /// </summary>
        public static List<K> ExecuteInParallel<T,K>(IEnumerable<T> elements, Func<T,K> operation, int numberOfThreads = 8)
        {
            ConcurrentBag<K> output = new ConcurrentBag<K>();

            int numberOfElements = elements.Count();
            int elementsPerThread = numberOfElements / numberOfThreads;

            List<Task> threads = new List<Task>();

            for (int i = 0; i < numberOfThreads; i++)
            {
                int tempI = i;
                threads.Add(Task.Factory.StartNew(() =>
                {
                    int start = tempI * elementsPerThread;
                    int end = tempI == numberOfThreads - 1 ? numberOfElements : start + elementsPerThread;

                    for (int j = start; j < end; j++)
                    {
                        output.Add(operation(elements.ElementAt(j)));
                    }
                }));
            }

            Task.WaitAll(threads.ToArray());

            return output.ToList();
        }
    }
}
