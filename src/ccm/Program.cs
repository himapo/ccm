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
            //using (Game1 game = new Game1())
            //{
            //    game.Run();
            //}

            using (var game = new HimaLib.System.Game())
            {
                var rootObject = new System.RootObject()
                {
                    CurrentScene = new Scene.FakeScene()
                };
                game.Run(rootObject, rootObject);
            }
        }
    }
#endif
}

