using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public class Game : IDisposable
    {
        public IGameInitializer Initializer { get; set; }

        public bool IsActive { get { return XnaGame.IsActive; } }

        XnaGame XnaGame;

        public Game()
        {
            Initializer = new DefaultGameInitializer();
        }

        public void Dispose()
        {
        }

        public void Run(IUpdater RootUpdater, IDrawer RootDrawer)
        {
            using (var game = new XnaGame(Initializer))
            {
                XnaGame = game;
                game.RootUpdater = RootUpdater;
                game.RootDrawer = RootDrawer;
                game.Run();
            }
        }
    }
}
