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
        }

        public bool IsPush(int label)
        {
            return false;
        }

        public bool IsPress(int label)
        {
            return false;
        }

        public bool IsRelease(int label)
        {
            return false;
        }

        public void AddDigitalDevice(int label, IDigitalDevice device)
        {
            digitalDevices[label] = device;
        }
    }
}
