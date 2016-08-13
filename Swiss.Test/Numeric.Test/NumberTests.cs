using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Swiss.Test
{
    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        [TestCategory("Numeric")]
        public void TestGenerateMultiples()
        {
            int x = 2;

            int[] expected = new int[] { 2, 4, 6, 8, 10 };
            int[] actual = x.GenerateMultiples(10).ToArray();

            CollectionAssert.AreEqual(actual, expected);
        }
    }
}
