using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    interface IDebugMenuService
    {
        bool AddChild(string parent, DebugMenuNode node);

        void Open();

        void Close();

        void Advance(DebugMenuNode node);

        void Back();

        void CursorUp();

        void CursorDown();
    }
}
