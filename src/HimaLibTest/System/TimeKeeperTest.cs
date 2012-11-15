using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace HimaLib.System
{
    public class TestingTimeKeeper : TimeKeeper
    {
        public override float LastFrameSeconds { get; set; }
    }

    [TestFixture]
    public class TimeKeeperTest
    {
        [TestCase]
        public void NormalLoad()
        {
            TestingTimeKeeper timeKeeper = new TestingTimeKeeper();

            timeKeeper.FrameRate = 60.0f;
            timeKeeper.LastFrameSeconds = 0.01667f;

            Assert.AreEqual(0.01667, timeKeeper.LastFrameSeconds, 0.0001);
            Assert.AreEqual(60.0, timeKeeper.RealFrameRate, 0.1);
            Assert.AreEqual(1.0, timeKeeper.LastTimeScale, 0.001);
        }
    }
}
