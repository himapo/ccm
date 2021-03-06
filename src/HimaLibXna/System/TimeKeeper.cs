﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.System
{
    public class TimeKeeper
    {
        public float FrameRate { get; set; }

        public float RealFrameRate
        {
            get
            {
                return 1.0f / LastFrameSeconds;
            }
        }

        public float AverageFrameRate { get; private set; }

        public float UpdateInterval { get; set; }

        public virtual float LastFrameSeconds
        {
            get { return (float)XnaGameTime.ElapsedGameTime.TotalSeconds; }
            set { }
        }

        public virtual float LastFrameMilliSeconds
        {
            get { return (float)XnaGameTime.ElapsedGameTime.TotalMilliseconds; }
            set { }
        }

        public float LastTimeScale
        {
            get
            {
                return FrameRate * LastFrameSeconds;
            }
        }

        public Microsoft.Xna.Framework.GameTime XnaGameTime { get; set; }

        static readonly TimeKeeper instance = new TimeKeeper();

        float totalTime;

        int totalFrame;

        public static TimeKeeper Instance { get { return instance; } set { } }

        protected TimeKeeper()
        {
            FrameRate = 60.0f;
            AverageFrameRate = 60.0f;
            UpdateInterval = 0.5f;
        }

        public void Update()
        {
            totalTime += LastFrameSeconds;
            totalFrame++;
            if (totalTime > UpdateInterval)
            {
                AverageFrameRate = totalFrame / totalTime;
                totalTime = 0.0f;
                totalFrame = 0;
            }
        }
    }
}
