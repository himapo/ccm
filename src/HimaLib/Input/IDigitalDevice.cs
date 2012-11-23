using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public interface IDigitalDevice
    {
        void Update();

        bool IsPush();

        bool IsPress();

        bool IsRelease();
    }
}
