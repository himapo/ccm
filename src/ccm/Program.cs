using System;

namespace ccm
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリー ポイントです。
        /// </summary>
        static void Main(string[] args)
        {
            using (var game = new HimaLib.System.Game())
            {
                var rootObject = new System.RootObject()
                {
                    CurrentScene = new Scene.BootScene()
                };
                game.Run(rootObject, rootObject);
            }
        }
    }
#endif
}

