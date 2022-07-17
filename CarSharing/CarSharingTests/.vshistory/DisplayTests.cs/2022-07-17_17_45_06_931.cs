using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class DisplayTests
    {
        [TestMethod()]
        public void IsUserNameAcceptableTest()
        {
            string faultyUsername = "!!!";
            string correctUsername = "Uwe123";
            Assert.IsFalse(IsUserNameAcceptable(faultyUsername));
            Assert.Fail();
        }
    }
}