using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ccmTest
{
    class SampleClass
    {
        public SampleClass()
        {
        }

        public bool PublicMethod()
        {
            return true;
        }

        private bool PrivateMethod()
        {
            return true;
        }
    }

    [TestFixture]
    public class SampleTest
    {
        [TestCase]
        public void Sample()
        {
            var sample = new SampleClass();
            Assert.True(sample.PublicMethod());
            Assert.True((bool)sample.CallPrivateMethod("PrivateMethod", new object[] { }));
        }
    }
}
