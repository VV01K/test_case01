using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace birthday_greeting.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void SendEmailTest()
        {
            Program.SendGreetings();
            Assert.AreEqual(1, Program.SentGreetings.Count);
        }
    }
}