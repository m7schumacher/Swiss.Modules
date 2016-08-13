using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Swiss;

namespace Swiss.Test
{
    [TestClass]
    public class IDictionaryTests
    {
        private Dictionary<int, double[]> MapsToCollection;
        private Dictionary<int, int> Basic;

        [TestInitialize]
        public void Initialize()
        {
            MapsToCollection = new Dictionary<int, double[]>()
            {
                { 1, new double[] { 1, 2 } },
                { 2, new double[] { 3, 4 } },
                { 3, new double[] { 5, 6 } }
            };

            Basic = new Dictionary<int, int>()
            {
                { 1, 6 },
                { 3, 5 },
                { 2, 4 }
            };
        }

        [TestMethod]
        public void TestGatherValues()
        {
            List<double> expected = new List<double>() { 1, 2, 3, 4, 5, 6 };
            List<double> actual = MapsToCollection.GatherAggregateOfValues();

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestIncrementOrAddNewRange()
        {
            double[] contents = new double[] { 1, 2, 3, 2, 3, 4, 5};

            Dictionary<double, int> expected = new Dictionary<double, int>()
            {
                { 1, 1 },
                { 2, 2 },
                { 3, 2 },
                { 4, 1 },
                { 5, 1 },
            };

            Dictionary<double, int> actual = new Dictionary<double, int>();
            actual.IncrementOrAddNewRange(contents);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestSortByKey()
        {
            Dictionary<int, int> expected = new Dictionary<int, int>()
            {
                { 1, 6 },
                { 2, 4 },
                { 3, 5 }
            };

            Dictionary<int, int> actual = Basic.SortByKey(d => d);
            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestSortByValue()
        {
            Dictionary<int, int> expected = new Dictionary<int, int>()
            {
                { 2, 4 },
                { 3, 5 },
                { 1, 6 }
            };

            Dictionary<int, int> actual = Basic.SortByValue(d => d);
            CollectionAssert.AreEqual(actual, expected);
        }
    }
}
