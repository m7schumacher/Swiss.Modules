using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using componentModel = System.ComponentModel;

namespace Swiss.Test
{
    [TestClass]
    public class EnumTests
    {
        public enum Mood
        {
            [componentModel.Description("A good mood")]
            Happy,
            Sad,
            Joyful,
            Gloom,
            Angry,
            Down,
            Up,
            Anxious
        }

        [TestMethod]
        public void TestGetName()
        {
            var target = Mood.Sad;
            Assert.AreEqual("Sad", target.ToString());
            Assert.AreEqual("Sad", Enum.GetName(target.GetType(), target));
            Assert.AreEqual("Sad", target.GetName());
        }

        [TestMethod]
        public void TestGetDescription()
        {
            var target = Mood.Happy;
            Assert.AreEqual("A good mood", target.GetDescription());
            Assert.AreEqual("Sad", Mood.Sad.GetDescription());
        }
    }
}
