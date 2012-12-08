using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugSample;
using Microsoft.Xna.Framework;

namespace HimaLib.Debug
{
    public class DebugSampleAccessor
    {
        static DebugSampleAccessor instance;

        public DebugManager DebugManager { get; private set; }

        public TimeRuler TimeRuler { get; private set; }

        public static void CreateInstance(Game game)
        {
            instance = new DebugSampleAccessor(game);
        }

        public static DebugSampleAccessor GetInstance()
        {
            return instance;
        }

        DebugSampleAccessor(Game game)
        {
            DebugManager = new DebugManager(game);
            game.Components.Add(DebugManager);

            TimeRuler = new TimeRuler(game);
            game.Components.Add(TimeRuler);
        }
    }
}
