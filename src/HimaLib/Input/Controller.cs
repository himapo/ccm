using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class Controller : IController
    {
        Dictionary<int, IBooleanDevice> booleanDevices;

        Dictionary<int, IPointingDevice> pointingDevices;

        Dictionary<int, IDigitalDevice> digitalDevices;

        public Controller()
        {
            booleanDevices = new Dictionary<int, IBooleanDevice>();
            pointingDevices = new Dictionary<int, IPointingDevice>();
            digitalDevices = new Dictionary<int, IDigitalDevice>();
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
            return booleanDevices[label].IsPush();
        }

        public bool IsPress(int label)
        {
            return booleanDevices[label].IsPress();
        }

        public bool IsRelease(int label)
        {
            return booleanDevices[label].IsRelease();
        }

        public int GetX(int label)
        {
            return pointingDevices[label].X;
        }

        public int GetY(int label)
        {
            return pointingDevices[label].Y;
        }

        public int GetMoveX(int label)
        {
            return pointingDevices[label].MoveX;
        }

        public int GetMoveY(int label)
        {
            return pointingDevices[label].MoveY;
        }

        public int GetDigitalValue(int label)
        {
            return digitalDevices[label].Value;
        }

        public int GetDigitalDelta(int label)
        {
            return digitalDevices[label].Delta;
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
    }
}
