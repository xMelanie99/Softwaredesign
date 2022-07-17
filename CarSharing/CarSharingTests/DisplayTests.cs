using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass()]
    public class DisplayTests
    {
        [TestMethod()]
        public void FaultyUsernameTest()
        {
            string faultyUsername = "!!!";
            // Assert.IsFalse = Schau ob false zurückgegeben wird (Testet ob der input inkorrekt ist oder nicht = nicht korrekt) 
            Assert.IsFalse(Display.GetInstance().IsUserNameAcceptable(faultyUsername));
        }
        [TestMethod()]
        public void CorrectUsernameTest()
        { 
            string correctUsername = "Meli123";
            // Assert.IsTrue = Schaut ob true zurückgegeben wird (korrekt)
            Assert.IsTrue(Display.GetInstance().IsUserNameAcceptable(correctUsername));
        }
    }
}