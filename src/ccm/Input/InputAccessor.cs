using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Input;

namespace ccm.Input
{
    public static class InputAccessor
    {
        public static void AddController(ControllerLabel controllerLabel, Controller controller, bool on)
        {
            HimaLib.Input.Input.AddController((int)controllerLabel, controller, on);
        }

        public static Controller GetController(ControllerLabel controllerLabel)
        {
            return HimaLib.Input.Input.GetController((int)controllerLabel);
        }

        public static void SwitchController(ControllerLabel controllerLabel, bool on)
        {
            HimaLib.Input.Input.SwitchController((int)controllerLabel, on);
        }

        public static void Update()
        {
            HimaLib.Input.Input.Update();
        }

        public static bool IsPush(ControllerLabel controllerLabel, BooleanDeviceLabel keyLabel)
        {
            return HimaLib.Input.Input.IsPush((int)controllerLabel, (int)keyLabel);
        }

        public static bool IsPress(ControllerLabel controllerLabel, BooleanDeviceLabel keyLabel)
        {
            return HimaLib.Input.Input.IsPress((int)controllerLabel, (int)keyLabel);
        }

        public static bool IsRelease(ControllerLabel controllerLabel, BooleanDeviceLabel keyLabel)
        {
            return HimaLib.Input.Input.IsRelease((int)controllerLabel, (int)keyLabel);            
        }

        public static int GetX(ControllerLabel controllerLabel, PointingDeviceLabel deviceLabel)
        {
            return HimaLib.Input.Input.GetX((int)controllerLabel, (int)deviceLabel);
        }

        public static int GetY(ControllerLabel controllerLabel, PointingDeviceLabel deviceLabel)
        {
            return HimaLib.Input.Input.GetY((int)controllerLabel, (int)deviceLabel);
        }
    }
}
