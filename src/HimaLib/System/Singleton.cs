using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public class Singleton<T>
        where T : class
    {
        static readonly T instance = null;

        public static T Instance { get { return instance; } }

        static Singleton()
        {
            var t = typeof(T);
            var obj = Activator.CreateInstance(t, true);
            instance = obj as T;
        }
    }
}
