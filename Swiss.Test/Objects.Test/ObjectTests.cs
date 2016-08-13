using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Swiss.Test
{
    [TestClass]
    public class ObjectTests
    {
        [TestMethod]
        public void TestGetPrivateMember()
        {
            Bar br = new Bar();

            string expected = "I am hidden";
            string actual = (string)br.GetPrivateProperty("Hidden");

            Assert.AreEqual(actual, expected);
        }
    }
}
