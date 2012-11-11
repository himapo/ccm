using System;
using System.Collections.Generic;

namespace ccm
{
    /// <summary>
    /// ゲームと関係のあるユーティリティ
    /// </summary>
    static class GameUtil
    {
        public static float FrameToSecond(int frame)
        {
            return (float)frame / GameProperty.GAME_FPS;
        }

        public static float FrameToMilliSecond(int frame)
        {
            return FrameToSecond(frame * 1000);
        }

    }
}
