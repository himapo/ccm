using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Debug
{
    public interface IDebugFont
    {
        void Initialize(string fontName);

        void Clear();

        void Add(string output, float x, float y);

        void Add(string output, float x, float y, Color fontColor, Color bgColor);

        void Draw();
    }
}
