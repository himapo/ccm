using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    class DebugMenuNode
    {
        Game Game { get; set; }

        public virtual string Label { get; set; }

        public bool Selectable { get; set; }

        public virtual DebugMenuNode SelectedChild { get; private set; }

        public DebugMenuNode(Game game)
        {
            this.Game = game;

            Label = "";
            Selectable = true;
        }

        protected ServiceType GetService<ServiceType>()
        {
            return (ServiceType)Game.Services.GetService(typeof(ServiceType));
        }

        public virtual void OnPushOK()
        {
        }

        public virtual void OnPushCancel()
        {
            GetService<IDebugMenuService>().Back();
        }

        public virtual void OnPushUp()
        {
            GetService<IDebugMenuService>().CursorUp();
        }

        public virtual void OnPushDown()
        {
            GetService<IDebugMenuService>().CursorDown();
        }

        public virtual void OnPushLeft()
        {
        }

        public virtual void OnPushRight()
        {
        }

        public virtual void CursorUp()
        {
        }

        public virtual void CursorDown()
        {
        }

        public virtual void DrawMenu()
        {
        }
    }
}
