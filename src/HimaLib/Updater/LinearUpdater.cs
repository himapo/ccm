using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Updater
{
    public class LinearUpdater : IUpdater
    {
        public bool Finish { get; set; }

        public float Start { get; set; }
        public float End { get; set; }
        public float Now { get; set; }

        public float FinishMilliSeconds { get; set; }
        public float ElapsedMilliSeconds { get; set; }

        Action<float> outputFunc;
        Action finishCallback;

        public LinearUpdater(
            IUpdaterManager updaterService,
            float finishMilliSeconds,
            float start,
            float end, 
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

            outputFunc(Now);

            Finish = (ElapsedMilliSeconds >= FinishMilliSeconds);
            if (Finish)
                finishCallback();
        }
    }
}
