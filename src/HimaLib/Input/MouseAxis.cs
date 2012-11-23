using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                return X - prevX;
            }
        }

        public int MoveY
        {
            get
            {
                return Y - prevY;
            }
        }

        IMouse mouse;

        int prevX;

        int prevY;

        public MouseAxis(IMouse mouse)
        {
            this.mouse = mouse;
            prevX = 0;
            prevY = 0;
        }

        public void Update()
        {
            prevX = X;
            prevY = Y;

            X = mouse.X;
            Y = mouse.Y;
        }
    }
}
