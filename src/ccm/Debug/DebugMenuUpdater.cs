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
            if (IsPush(DigitalDeviceLabel.ToggleDebugMenu))
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

            if (IsPush(DigitalDeviceLabel.Up))
            {
                DebugMenu.OnPushUp();
            }
            if (IsPush(DigitalDeviceLabel.Down))
            {
                DebugMenu.OnPushDown();
            }
            if (IsPush(DigitalDeviceLabel.Left))
            {
                DebugMenu.OnPushLeft();
            }
            if (IsPush(DigitalDeviceLabel.Right))
            {
                DebugMenu.OnPushRight();
            }
            if (IsPush(DigitalDeviceLabel.OK))
            {
                DebugMenu.OnPushOK();
            }
            if (IsPush(DigitalDeviceLabel.Cancel))
            {
                DebugMenu.OnPushCancel();
            }
        }

        bool IsPush(DigitalDeviceLabel key)
        {
            return InputAccessor.IsPush(ControllerLabel.Debug, key);
        }
    }
}
