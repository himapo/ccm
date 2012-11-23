using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class Controller
    {
        Dictionary<int, IDigitalDevice> digitalDevices;

        Dictionary<int, IPointingDevice> pointingDevices;

        public Controller()
        {
            digitalDevices = new Dictionary<int, IDigitalDevice>();
            pointingDevices = new Dictionary<int, IPointingDevice>();
        }

        public void Update()
        {
            foreach (var device in digitalDevices.Values)
            {
                device.Update();
            }

            foreach (var device in pointingDevices.Values)
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

        public int GetX(int label)
        {
            return pointingDevices[label].X;
        }

        public int GetY(int label)
        {
            return pointingDevices[label].Y;
        }

        public void AddDigitalDevice(int label, IDigitalDevice device)
        {
            digitalDevices[label] = device;
        }

        public void AddPointingDevice(int label, IPointingDevice device)
        {
            pointingDevices[label] = device;
        }
    }
}
