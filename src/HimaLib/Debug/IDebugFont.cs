using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;

namespace HimaLib.Debug
{
    public interface IDebugFont
    {
        void Initialize(string fontName);

        void Clear();

        void Add(string output, float x, float y);

        void Add(string output, float x, float y, IColor fontColor, IColor bgColor);

        void Draw();
    }
}
