using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Input;
using ccm.Input;

namespace ccm.Debug
{
    public class DebugMenuUpdater
    {
        public IDebugMenu DebugMenu { get; set; }

        public DebugMenuUpdater(IDebugMenu debugMenu)
        {
            DebugMenu = debugMenu;
        }

        public void Update()
        {
            if (IsPush(VirtualKeyLabel.ToggleDebugMenu))
            {
                if (DebugMenu.IsOpen)
                {
                    DebugMenu.Close();
                }
                else
                {
                    DebugMenu.Open();
                }
                InputAccessor.SwitchController(ControllerLabel.Main, !DebugMenu.IsOpen);
                return;
            }

            if (!DebugMenu.IsOpen)
            {
                return;
            }

            if (IsPush(VirtualKeyLabel.Up))
            {
                DebugMenu.OnPushUp();
            }
            if (IsPush(VirtualKeyLabel.Down))
            {
                DebugMenu.OnPushDown();
            }
            if (IsPush(VirtualKeyLabel.Left))
            {
                DebugMenu.OnPushLeft();
            }
            if (IsPush(VirtualKeyLabel.Right))
            {
                DebugMenu.OnPushRight();
            }
            if (IsPush(VirtualKeyLabel.OK))
            {
                DebugMenu.OnPushOK();
            }
            if (IsPush(VirtualKeyLabel.Cancel))
            {
                DebugMenu.OnPushCancel();
            }
        }

        bool IsPush(VirtualKeyLabel key)
        {
            return InputAccessor.IsPush(ControllerLabel.Debug, key);
        }
    }
}
