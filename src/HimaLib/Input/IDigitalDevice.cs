﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public interface IDigitalDevice
    {
        int Value { get; }

        int Delta { get; }

        void Update();
    }
}
