using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Updater
{
    public class CylinderUpdater
    {
        public CylinderUpdater(
            IUpdaterManager updaterService,
            float finishMilliSeconds,
            float startDegree,
            float endDegree,
            Vector2 center,
            Vector2 amplitude,
            Action<float> outputX,
            Action<float> outputY,
            float startHeight,
            float endHeight,
            Action<float> outputHeight,
            Action callback
            )
        {
            new SinUpdater(
                updaterService,
                finishMilliSeconds,
                startDegree,
                endDegree,
                center.X,
                amplitude.X,
                outputX,
                () => { });

            new SinUpdater(
                updaterService,
                finishMilliSeconds,
                startDegree + 90.0f,
                endDegree + 90.0f,
                center.Y,
                amplitude.Y,
                outputY,
                () => { });

            new LinearUpdater(
                updaterService,
                finishMilliSeconds,
                startHeight,
                endHeight,
                outputHeight,
                callback);
        }
    }
}
