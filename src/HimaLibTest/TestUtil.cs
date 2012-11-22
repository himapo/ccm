using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HimaLib
{
    public static class TestUtil
    {
        public static ReturnType CallPrivateMethod<ReturnType>(this object instance, string methodName, params object[] args)
        {
            var type = instance.GetType();

            var methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            
            return (ReturnType)methodInfo.Invoke(instance, args);  
        }
    }
}
