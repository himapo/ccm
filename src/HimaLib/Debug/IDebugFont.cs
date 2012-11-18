using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public interface IDebugFont
    {
        void Initialize(string fontName);

        void Clear();

        void Add(string output, float x, float y);

        void Draw();
    }
}
