using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace HimaLib.Debug
{
    public class DebugFont : DebugFontBase
    {
        public static void Create()
        {
            Instance = new DebugFont();
        }

        DebugFont()
        {
        }

        protected override Font CreateFont(string output)
        {
            return new FontXna()
            {
                String = output,
            };
        }

        public static void Add(string output, float x, float y)
        {
            Instance.Draw(output, x, y);
        }

        public static void Add(string output, float x, float y, Color fontColor, Color bgColor)
        {
            Instance.Draw(output, x, y, fontColor, bgColor);
        }
    }
}
