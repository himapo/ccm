using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Updater
{
    public class CircleUpdater
    {
        public CircleUpdater(
            IUpdaterManager updaterService,
            float start,
            float end,
            float finishMilliSeconds,
            Vector2 center,
            Vector2 amplitude,
            Action<float> outputX,
            Action<float> outputY,
            Action callback
            )
        {
            new SinUpdater(
                updaterService,
                start,
                end,
                finishMilliSeconds,
                center.X,
                amplitude.X,
                outputX,
                () => { });

            new SinUpdater(
                updaterService,
                start + 90.0f,
                end + 90.0f,
                finishMilliSeconds,
                center.Y,
                amplitude.Y,
                outputY,
                callback);
        }
    }
}
