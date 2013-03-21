using System;
using System.Collections.Generic;

namespace ccm.Util
{
    /// <summary>
    /// ゲームと関係のあるユーティリティ
    /// </summary>
    static class GameUtil
    {
        public static float FrameToSecond(int frame)
        {
            return (float)frame / Game.GameProperty.GAME_FPS;
        }

        public static float FrameToMilliSecond(int frame)
        {
            return FrameToSecond(frame * 1000);
        }

    }
}
