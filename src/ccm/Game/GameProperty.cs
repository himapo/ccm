using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm.Game
{
    public static class GameProperty
    {
        public const float GAME_FPS = 60.0f;
        public const float CUBE_WIDTH = 3.0f;

        public static float fov;
        public static int resolutionWidth;
        public static int resolutionHeight;

        static GameProperty()
        {
            fov = 30.0f;
            resolutionWidth = 1280;
            resolutionHeight = 720;
        }

        public static void Load()
        {
        }

        public static void Save()
        {
        }
    }
}
