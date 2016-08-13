using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.Test
{
    [TestClass]
    public class ParallelTests
    {
        string[] elements; 

        [TestInitialize]
        public void Initialize()
        {
            elements = new string[]
            {
                "test", "this is a test", "this is not"
            };
        }

        [TestMethod]
        public void TestParallelAction()
        {
            ConcurrentBag<string> validPages = new ConcurrentBag<string>();
            ConcurrentBag<string> invalidPages = new ConcurrentBag<string>();

            Action<string> filter = (string input) =>
            {
                if (input.Contains("test"))
                    validPages.Add(input);
                else
                    invalidPages.Add(input);
            };
           
            elements.ExecuteInParallel(filter);

            List<string> expectedValid = new List<string>()
            {
                "test", "this is a test"
            };

            List<string> expectedInvalid = new List<string>()
            {
                "this is not"
            };

            var valids = validPages.OrderByAbc().ToList();

            CollectionAssert.AreEqual(valids, expectedValid.ToList());
            CollectionAssert.AreEqual(invalidPages, expectedInvalid);
        }

        [TestMethod]
        public void TestParallelFunction()
        {
            Func<string, bool> funcFilter = (string input) =>
            {
                return input.Contains("test");
            };

            int expectedTrue = 2;
            int expectedFalse = 1;

            bool[] actual = ParallelUtility.ExecuteInParallel(elements, funcFilter).ToArray();

            Assert.AreEqual(expectedTrue, actual.Count(elem => elem == true));
            Assert.AreEqual(expectedFalse, actual.Count(elem => elem == false));
        }
    }
}
