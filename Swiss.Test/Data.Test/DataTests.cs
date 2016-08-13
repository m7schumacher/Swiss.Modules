using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Swiss.Test
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void TestFolderGeneration()
        {
            string onDesktop = Folders.MakePath(Folders.CommonFolders.Desktop, "test.txt");
        }
    }
}
