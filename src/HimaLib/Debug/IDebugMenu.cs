using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Debug
{
    public interface IDebugMenu
    {
        bool IsOpen { get; }

        bool AddChild(string parent, DebugMenuNode node);

        void Open();

        void Close();

        void OnPushOK();

        void OnPushCancel();

        void OnPushUp();

        void OnPushDown();

        void OnPushLeft();

        void OnPushRight();

        void Draw(IDebugMenuDrawer drawer);
    }
}
