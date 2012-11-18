using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public static class DebugFont
    {
#if DEBUG
        static SpriteDebugFont instance = new SpriteDebugFont();
#else
        static NullDebugFont instance = new NullDebugFont();
#endif

        static IDebugFont GetInstance()
        {
            return instance;
        }

        public static void Initialize(string fontName)
        {
            GetInstance().Initialize(fontName);
        }

        public static void Clear()
        {
            GetInstance().Clear();
        }

        public static void Add(string output, float x, float y)
        {
            GetInstance().Add(output, x, y);
        }

        public static void Draw()
        {
            GetInstance().Draw();
        }
    }
}
