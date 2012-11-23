using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class Controller
    {
        Dictionary<int, IDigitalDevice> digitalDevices;

        public Controller()
        {
            digitalDevices = new Dictionary<int, IDigitalDevice>();
        }

        public void Update()
        {
            foreach (var device in digitalDevices.Values)
            {
                device.Update();
            }
        }

        public bool IsPush(int label)
        {
            return digitalDevices[label].IsPush();
        }

        public bool IsPress(int label)
        {
            return digitalDevices[label].IsPress();
        }

        public bool IsRelease(int label)
        {
            return digitalDevices[label].IsRelease();
        }

        public void AddDigitalDevice(int label, IDigitalDevice device)
        {
            digitalDevices[label] = device;
        }
    }
}
