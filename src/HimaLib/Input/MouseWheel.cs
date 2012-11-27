using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public class MouseWheel : IDigitalDevice
    {
        public int Value { get; private set; }

        public int Delta
        {
            get
            {
                return Value - prevWheel;
            }
        }

        IMouse mouse;

        int prevWheel;

        public MouseWheel(IMouse mouse)
        {
            this.mouse = mouse;
            prevWheel = 0;
        }

        public void Update()
        {
            prevWheel = Value;

            Value = mouse.Wheel;
        }
    }
}
