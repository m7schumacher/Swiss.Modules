using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Swiss;

namespace Swiss.Test
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        public void TestTrimCharactersWithin()
        {
            string original = "abcdefghijkl";
            string expected = "abcdeijkl";

            char[] remove = new char[] { 'f', 'g', 'h' };
            string actual = original.TrimCharactersWithin(remove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTrimCharacterFromEnd()
        {
            string original = "abcdefghijkl";
            string expected = "abcdefgh";

            char[] remove = new char[] { 'i', 'j', 'k', 'l' };
            string actual = original.TrimCharactersFromEnd(remove);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTrimPunctionationFromEnd()
        {
            string original = "this?!.";
            string expected = "this";

            string actual = original.TrimBasicPunctuationFromEnd();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTrimPunctionationWithin()
        {
            string original = "t!h.is?!.";
            string expected = "this";

            string actual = original.TrimPunctuation();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIsUpperCase()
        {
            string isUpper = "This";
            string isLower = "this";

            Tuple<bool, bool> expected = new Tuple<bool, bool>(true, false);
            Tuple<bool, bool> actual = new Tuple<bool, bool>(isUpper.IsUpperCase(), isLower.IsUpperCase());
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetAllDigits()
        {
            string original = "t2h3h53l4";
            string[] expected = new string[] { "2", "3", "53", "4" };

            string[] actual = original.GetAllDigits();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetAllWords()
        {
            string original = "this is a test exit!";
            string[] expected = new string[] { "this", "is", "a", "test", "exit" };

            string[] actual = original.GetAllWords();

            CollectionAssert.AreEqual(expected, actual);
        }

        public void TestTrimBlanks()
        {
            string[] original = new string[] { "this", "", "a", "", "exit" };
            string[] expected = new string[] { "this", "a", "exit" };

            string[] actual = original.WhereNotEmpty().ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetAllUpperCaseWords()
        {
            string original = "This is a Test Exit!";
            string[] expected = new string[] { "This", "Test", "Exit" };
            string[] actual = original.GetAllUpperCasedWords();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetQuotes()
        {
            string original = "he said \"where are you?\" and 'where'";
            string[] expected = new string[] { "where are you?" };
            string[] actual = original.GetTextInDoubleQuotes();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetSingleQuotes()
        {
            string original = "he said 'where are you?' and 'where'";
            string[] expected = new string[] { "where are you?", "where" };
            string[] actual = original.GetTextInSingleQuotes();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetAllQuotes()
        {
            string original = "he said \"where are you?\" and 'where'";
            string[] expected = new string[] { "where are you?", "where" };
            string[] actual = original.GetTextInAllQuotes();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSplitWhiteSpace()
        {
            string original = "he was like yeah!";
            string[] expected = new string[] { "he", "was", "like", "yeah!" };
            string[] actual = original.SplitOnWhiteSpace();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSplitOnDigits()
        {
            string original = "he was 10 like yeah!";
            string[] expected = new string[] { "he was", "like yeah!" };
            string[] actual = original.SplitOnDigits();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSplitOnNewLine()
        {
            string original = "he was\nlike\nyeah!";
            string[] expected = new string[] { "he was", "like", "yeah!" };
            string[] actual = original.SplitOnNewLines();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestUpperCaseFirstLetter()
        {
            string original = "he was like yeah";
            string expected = "He Was Like Yeah";
            string actual = original.UpperCaseFirstLetters();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void testRemoveSpaces()
        {
            string original = "he was like yeah";
            string expected = "hewaslikeyeah";
            string actual = original.RemoveSpaces();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRemove()
        {
            string original = "he was like yeah";
            string expected = "he was like yh";
            string actual = original.Remove("ea");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestOrValue()
        {
            string original = null;
            string expected = "success!";

            string actual = original.OrValue("success!");

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestEqualsIgnoreCase()
        {
            string original = "tst";
            string original1 = "TesT";

            string test = "test";

            Tuple<bool, bool> expected = new Tuple<bool, bool>(false, true);
            Tuple<bool, bool> actual = new Tuple<bool, bool>(original.EqualsIgnoreCase(test), original1.EqualsIgnoreCase(test));

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestContainsIgnoreCase()
        {
            string original = "tstafdafse";
            string original1 = "asdfTesTasdfa";

            string test = "test";

            Tuple<bool, bool> expected = new Tuple<bool, bool>(false, true);
            Tuple<bool, bool> actual = new Tuple<bool, bool>(original.ContainsIgnoreCase(test), original1.ContainsIgnoreCase(test));

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestStartsWithIgnoreCase()
        {
            string original = "tstasdfdsa";
            string original1 = "TesTasdfsdaf";

            string test = "test";

            Tuple<bool, bool> expected = new Tuple<bool, bool>(false, true);
            Tuple<bool, bool> actual = new Tuple<bool, bool>(original.StartsWithIgnoreCase(test), original1.StartsWithIgnoreCase(test));

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestEndsWithIgnoreCase()
        {
            string original = "dsafdasfasdtst";
            string original1 = "asdfasdfaTesT";

            string test = "test";

            Tuple<bool, bool> expected = new Tuple<bool, bool>(false, true);
            Tuple<bool, bool> actual = new Tuple<bool, bool>(original.EndsWithIgnoreCase(test), original1.EndsWithIgnoreCase(test));

            Assert.AreEqual(actual, expected);
        }
    }
}
