using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ccmTest
{
    static class TestUtil
    {
        public static object CallPrivateMethod(this object instance, string methodName, object[] args)
        {
            var type = instance.GetType();

            var methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            
            return methodInfo.Invoke(instance, args);  
        }
    }
}
