using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace HimaLib.Input
{
    class MouseWheelTest
    {
        FakeMouse mouse;

        MouseWheel instance;

        [SetUp]
        protected void SetUp()
        {
            mouse = new FakeMouse();
            instance = new MouseWheel(mouse);
        }

        [TestCase]
        public void GetPosition()
        {
            Assert.AreEqual(0, instance.Value);

            mouse.Wheel = 505;
            instance.Update();
            Assert.AreEqual(505, instance.Value);
            Assert.AreEqual(505, instance.Delta);

            mouse.Wheel = -124;
            instance.Update();
            Assert.AreEqual(-124, instance.Value);
            Assert.AreEqual(-629, instance.Delta);
        }
    }
}
