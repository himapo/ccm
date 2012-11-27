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
            if (IsPush(BooleanDeviceLabel.ToggleDebugMenu))
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

            if (IsPush(BooleanDeviceLabel.Up))
            {
                DebugMenu.OnPushUp();
            }
            if (IsPush(BooleanDeviceLabel.Down))
            {
                DebugMenu.OnPushDown();
            }
            if (IsPush(BooleanDeviceLabel.Left))
            {
                DebugMenu.OnPushLeft();
            }
            if (IsPush(BooleanDeviceLabel.Right))
            {
                DebugMenu.OnPushRight();
            }
            if (IsPush(BooleanDeviceLabel.OK))
            {
                DebugMenu.OnPushOK();
            }
            if (IsPush(BooleanDeviceLabel.Cancel))
            {
                DebugMenu.OnPushCancel();
            }
        }

        bool IsPush(BooleanDeviceLabel key)
        {
            return InputAccessor.IsPush(ControllerLabel.Debug, key);
        }
    }
}
