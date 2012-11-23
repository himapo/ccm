using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace HimaLib.Input
{
    class ControllerTest
    {
        Controller instance;

        [SetUp]
        protected void SetUp()
        {
            instance = new Controller();
        }

        [TestCase]
        public void DigitalDevice()
        {
            //Assert.False(instance.IsPush(0));
        }
    }
}
