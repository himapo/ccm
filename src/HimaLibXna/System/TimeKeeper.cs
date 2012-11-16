﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.System
{
    public class TimeKeeper : ITimeKeeper
    {
        public float FrameRate { get; set; }

        public float RealFrameRate
        {
            get
            {
                return 1.0f / LastFrameSeconds;
            }
        }

        public virtual float LastFrameSeconds
        {
            get
            {
                return (float)XnaGameTime.ElapsedGameTime.TotalSeconds;
            }
            set
            {
            }
        }

        public float LastTimeScale
        {
            get
            {
                return FrameRate * LastFrameSeconds;
            }
        }

        public Microsoft.Xna.Framework.GameTime XnaGameTime { get; set; }

        public TimeKeeper()
        {
            FrameRate = 60.0f;
        }

        public void Update()
        {
        }
    }
}