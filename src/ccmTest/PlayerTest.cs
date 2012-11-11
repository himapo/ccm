using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ccmTest
{
    [TestFixture]
    public class PlayerTest
    {
        [SetUp]
        public void SetUp()
        {
            ccm.Player.CreateInstance(null);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [TestCase]
        public void Instance()
        {
            Assert.NotNull(ccm.Player.GetInstance());
        }
    }
}
