using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Swiss.Test
{
    [TestClass]
    public class IEnumerableTests
    {
        Bar[] bars;

        [TestInitialize]
        public void Initialize()
        {
            Bar br = new Bar(5, 5);
            Bar br1 = new Bar(7, 6);
            Bar br2 = new Bar(10, 7);
            Bar br3 = new Bar(12, 8);
            Bar br4 = new Bar(15, 10);

            bars = new Bar[] { br, br1, br2, br3, br4 };
        }

        [TestMethod]
        public void TestMaxBy()
        {
            Bar expected = bars[4];
            Bar actual = bars.MaxOfField(bar => bar.X);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAlterWhere()
        {
            Bar[] expected = new Bar[]
            {
                new Bar(5, 10),
                new Bar(7, 6)
            };

            Bar[] actual = bars.Take(2).AlterWhere(br => br.X == 5, br => new Bar(br.X, 10)).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTransform()
        {
            string[] expected = new string[]
            {
                "5-5",
                "7-6"
            };

            string[] actual = bars.Take(2).Select(br => br.ToString()).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMinBy()
        {
            Bar expected = bars[0];
            Bar actual = bars.MinOfField(bar => bar.X);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestDistinctFields()
        {
            int[] expected = new int[] { 5, 7, 10, 12, 15 };
            int[] actual = bars.GetDistinctValuesOfField(b => b.X).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFirstIndexOf()
        {
            int[] scapegoat = new int[] { 5, 7, 10, 12, 15 };
            string[] test = new string[] { "this", "is", "a", "test" };

            int expected = 2;
            int actual = test.FirstIndexOf("a");

            Assert.AreEqual(expected, actual);

            expected = 2;
            actual = scapegoat.FirstIndexOf(10);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFieldsInNthPosition_Even()
        {
            Bar[] expected = new Bar[] { bars[0], bars[2], bars[4] };
            Bar[] actual = bars.GetFieldsInNthPositions(2).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFieldsInNthPosition_Odd()
        {
            Bar[] expected = new Bar[] { bars[0], bars[3] };
            Bar[] actual = bars.GetFieldsInNthPositions(3).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestJoinOnDelimiter()
        {
            string[] test = new string[] { "this", "is", "a", "test" };
            string expected = "this-is-a-test";
            string actual = test.ToString("-");

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestBecomeCountKeeper()
        {
            double[] contents = new double[] { 1, 2, 3, 2, 3, 4, 5 };

            Dictionary<double, int> expected = new Dictionary<double, int>()
            {
                { 1, 1 },
                { 2, 2 },
                { 3, 2 },
                { 4, 1 },
                { 5, 1 },
            };

            Dictionary<double, int> actual = contents.GetCountsOfDistinctFields(d => d);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestGridInversion()
        {
            double[][] original = new double[][]
            {
                new double[] { 1, 2, 3, 4 },
                new double[] { 5, 6, 7, 8 }
            };

            double[][] expected = new double[][]
            {
                new double[] { 1, 5 },
                new double[] { 2, 6 },
                new double[] { 3, 7 },
                new double[] { 4, 8 }
            };

            double[][] actual = original.Invert();

            Assert.AreEqual(actual.Length, expected.Length);

            for(int i = 0; i < actual.Length; i++)
            {
                CollectionAssert.AreEqual(actual[i], expected[i]);
            }
        }

        [TestMethod]
        public void TestWhereNotContains()
        {
            string[] test = new string[] { "this", "is", "a", "test" };

            string[] expected = new string[] { "is", "a" };
            string[] actual = test.WhereNotContained("t").ToArray();

            CollectionAssert.AreEqual(expected, actual);

            expected = new string[] { "a" };
            actual = test.WhereNoneContain(new string[] { "is", "te" }).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWhereNotEquals()
        {
            string[] test = new string[] { "this", "is", "a", "test" };

            string[] expected = new string[] { "is", "a", "test" };
            string[] actual = test.WhereNotEqual("this").ToArray();

            CollectionAssert.AreEqual(expected, actual);

            expected = new string[] { "a" };
            actual = test.WhereNoneEqual(new string[] { "this", "is", "test" }).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWhereGreaterThan()
        {
            int[] test = new int[] { 1, 2, 3, 4, 5 };
            int[] expected = new int[] { 3, 4, 5 };
            int[] actual = test.WhereGreaterThan(2).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWhereLessThan()
        {
            int[] test = new int[] { 1, 2, 3, 4, 5 };
            int[] expected = new int[] { 1, 2, 3 };
            int[] actual = test.WhereLessThan(4).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWhereBetween()
        {
            int[] test = new int[] { 1, 2, 3, 4, 5 };
            int[] expected = new int[] { 2, 3, 4 };
            int[] actual = test.WhereBetween(1, 5).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
