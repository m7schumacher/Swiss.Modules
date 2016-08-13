using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swiss;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Sandbox.Projects
{
    public class PerformanceTester : Project
    {
        public PerformanceTester()
        {

        }

        public override void Execute()
        {
            var result = DiagnosticUtility.DetermineFastestFunction(() => ExecuteLeft(), () => ExecuteRight(), 1000);
        }

        public void ExecuteLeft()
        {
            ConcurrentBag<string> strMine = new ConcurrentBag<string>();
            var range = Enumerable.Range(0, 100);

            range.ExecuteInParallel(num => strMine.Add(StringExtensions.GenerateRandomString(100)));
        }

        public void ExecuteRight()
        {
            ConcurrentBag<string> strTheirs = new ConcurrentBag<string>();
            var range = Enumerable.Range(0, 100);

            Parallel.ForEach(range, num => strTheirs.Add(StringExtensions.GenerateRandomString(100)));
        }
    }
}
