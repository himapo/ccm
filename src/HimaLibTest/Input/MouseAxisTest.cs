using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace HimaLib.Input
{
    class MouseAxisTest
    {
        FakeMouse mouse;

        MouseAxis instance;

        [SetUp]
        protected void SetUp()
        {
            mouse = new FakeMouse();
            instance = new MouseAxis(mouse);
        }

        [TestCase]
        public void GetPosition()
        {
            Assert.AreEqual(0, instance.X);
            Assert.AreEqual(0, instance.Y);

            // (5, -24) に移動
            mouse.X = 5;
            mouse.Y = -24;
            instance.Update();
            Assert.AreEqual(5, instance.X);
            Assert.AreEqual(-24, instance.Y);
            Assert.AreEqual(5, instance.MoveX);
            Assert.AreEqual(-24, instance.MoveY);

            // (-1531, 32) に移動
            mouse.X = -1531;
            mouse.Y = 32;
            instance.Update();
            Assert.AreEqual(-1531, instance.X);
            Assert.AreEqual(32, instance.Y);
            Assert.AreEqual(-1536, instance.MoveX);
            Assert.AreEqual(56, instance.MoveY);
        }
    }
}
