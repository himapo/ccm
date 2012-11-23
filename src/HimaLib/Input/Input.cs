using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public static class Input
    {
        static Dictionary<int, Controller> controllers = new Dictionary<int,Controller>();

        static Dictionary<int, bool> enables = new Dictionary<int, bool>();

        public static void AddController(int id, Controller controller, bool on)
        {
            controllers[id] = controller;
            enables[id] = on;
        }

        public static Controller GetController(int id)
        {
            return controllers[id];
        }

        public static void SwitchController(int id, bool on)
        {
            enables[id] = on;
        }

        public static void Update()
        {
            foreach (var controller in controllers.Values)
            {
                controller.Update();
            }
        }

        public static bool IsPush(int controllerId, int keyLabel)
        {
            return enables[controllerId] && controllers[controllerId].IsPush(keyLabel);
        }

        public static bool IsPress(int controllerId, int keyLabel)
        {
            return enables[controllerId] && controllers[controllerId].IsPress(keyLabel);
        }

        public static bool IsRelease(int controllerId, int keyLabel)
        {
            return enables[controllerId] && controllers[controllerId].IsRelease(keyLabel);
        }
    }
}
