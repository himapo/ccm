using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ccm
{
    /// <summary>
    /// あらゆるところから参照されるパラメータなど
    /// </summary>
    static class GameProperty
    {
        public const float GAME_FPS = 60.0f;
        public const float CUBE_WIDTH = 3.0f;

        public static float fov;
        public static int resolutionWidth;
        public static int resolutionHeight;

        public static IRand gameRand;
        public static IRand drawRand;

        static GameProperty()
        {
            fov = 30.0f;
            resolutionWidth = 1280;
            resolutionHeight = 720;

            gameRand = new SystemRand();
            drawRand = new SystemRand();
        }

        public static void Load()
        {
        }

        public static void Save()
        {
        }

        public static float GetUpdateScale(GameTime gameTime)
        {
            return GAME_FPS * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
