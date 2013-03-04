using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Updater
{
    public class SinUpdater : IUpdater
    {
        public bool Finish { get; set; }
        
        // 変化域は角度で指定する
        public float Start { get; set; }
        public float End { get; set; }
        public float Now { get; set; }

        public float FinishMilliSeconds { get; set; }
        public float ElapsedMilliSeconds { get; set; }

        public float Center { get; set; }
        public float Amplitude { get; set; }

        Action<float> outputFunc;
        Action finishCallback;

        public SinUpdater(
            IUpdaterManager updaterService,
            float finishMilliSeconds,
            float start,
            float end,
            float center,
            float amplitude,
            Action<float> output,
            Action callback
            )
        {
            outputFunc = output;
            finishCallback = callback;

            Start = Now = start;
            End = end;
            FinishMilliSeconds = finishMilliSeconds;
            ElapsedMilliSeconds = 0.0f;

            Amplitude = amplitude;
            Center = center;

            outputFunc(Start);

            updaterService.Add(this);
        }

        public void Update(float elapsedMilliSeconds)
        {
            ElapsedMilliSeconds += elapsedMilliSeconds;

            if (ElapsedMilliSeconds < 0.0f)
            {
                // 例外にすべき？
                Now = Start;
            }
            else if (ElapsedMilliSeconds > FinishMilliSeconds)
            {
                Now = End;
            }
            else
            {
                Now = (End * ElapsedMilliSeconds + Start * (FinishMilliSeconds - ElapsedMilliSeconds)) / FinishMilliSeconds;
            }

            outputFunc(MathUtil.Sin(MathUtil.ToRadians(Now)) * Amplitude + Center);

            Finish = (ElapsedMilliSeconds >= FinishMilliSeconds);
            if (Finish)
                finishCallback();
        }
    }
}
