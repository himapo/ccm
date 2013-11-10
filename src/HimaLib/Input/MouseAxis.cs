using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using HimaLib.Math;

namespace HimaLib.Input
{
    public class MouseAxis : IPointingDevice
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public int MoveX
        {
            get
            {
                return moveX;
            }
        }

        public int MoveY
        {
            get
            {
                return moveY;
            }
        }

        IMouse mouse;

        int prevX;

        int prevY;

        int moveX;

        int moveY;

        public MouseAxis(IMouse mouse)
        {
            this.mouse = mouse;
        }

        public void Update()
        {
            if (mouse.FixedAtCenter)
            {
                prevX = 0;
                prevY = 0;
            }
            else
            {
                prevX = X;
                prevY = Y;
            }

            var realX = mouse.X;
            var realY = mouse.Y;

            moveX = realX - prevX;
            moveY = realY - prevY;

            X = realX;
            Y = realY;

            if (mouse.FixedAtCenter)
            {
                mouse.X = 0;
                mouse.Y = 0;
            }
        }
    }
}
