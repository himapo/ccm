using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class Controller : IController
    {
        Dictionary<int, IBooleanDevice> booleanDevices = new Dictionary<int, IBooleanDevice>();

        Dictionary<int, IPointingDevice> pointingDevices = new Dictionary<int,IPointingDevice>();

        Dictionary<int, IDigitalDevice> digitalDevices = new Dictionary<int,IDigitalDevice>();

        public Controller()
        {
        }

        public void Update()
        {
            foreach (var device in booleanDevices.Values)
            {
                device.Update();
            }

            foreach (var device in pointingDevices.Values)
            {
                device.Update();
            }

            foreach (var device in digitalDevices.Values)
            {
                device.Update();
            }
        }

        public bool IsPush(int label)
        {
            return GetBooleanDevice(label).IsPush();
        }

        public bool IsPress(int label)
        {
            return GetBooleanDevice(label).IsPress();
        }

        public bool IsRelease(int label)
        {
            return GetBooleanDevice(label).IsRelease();
        }

        public int GetX(int label)
        {
            return GetPointingDevice(label).X;
        }

        public int GetY(int label)
        {
            return GetPointingDevice(label).Y;
        }

        public int GetMoveX(int label)
        {
            return GetPointingDevice(label).MoveX;
        }

        public int GetMoveY(int label)
        {
            return GetPointingDevice(label).MoveY;
        }

        public int GetDigitalValue(int label)
        {
            return GetDigitalDevice(label).Value;
        }

        public int GetDigitalDelta(int label)
        {
            return GetDigitalDevice(label).Delta;
        }

        public void AddBooleanDevice(int label, IBooleanDevice device)
        {
            booleanDevices[label] = device;
        }

        public void AddPointingDevice(int label, IPointingDevice device)
        {
            pointingDevices[label] = device;
        }

        public void AddDigitalDevice(int label, IDigitalDevice device)
        {
            digitalDevices[label] = device;
        }

        IBooleanDevice GetBooleanDevice(int label)
        {
            IBooleanDevice device;
            if (!booleanDevices.TryGetValue(label, out device))
            {
                device = new NullBooleanDevice();
                AddBooleanDevice(label, device);
            }
            return device;
        }

        IPointingDevice GetPointingDevice(int label)
        {
            IPointingDevice device;
            if (!pointingDevices.TryGetValue(label, out device))
            {
                device = new NullPointingDevice();
                AddPointingDevice(label, device);
            }
            return device;
        }

        IDigitalDevice GetDigitalDevice(int label)
        {
            IDigitalDevice device;
            if (!digitalDevices.TryGetValue(label, out device))
            {
                device = new NullDigitalDevice();
                AddDigitalDevice(label, device);
            }
            return device;
        }
    }
}
