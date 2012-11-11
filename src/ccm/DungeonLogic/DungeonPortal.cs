using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    /// <summary>
    /// 通路と部屋、通路同士をつなぐ接続部
    /// </summary>
    class DungeonPortal
    {
        public Point Position { get; set; }

        public DungeonRoom ConnectedRoom { get; set; }

        public List<DungeonPath> ConnectedPaths { get; set; }

        public bool Accessible { get; set; }

        public DungeonPortal(Point position)
        {
            Position = position;
            ConnectedPaths = new List<DungeonPath>();
            Accessible = false;
        }

        public void CheckAccessibility()
        {
            if (Accessible)
                return;

            Accessible = true;

            if (ConnectedRoom != null)
                ConnectedRoom.CheckAccessibility();

            foreach (var path in ConnectedPaths)
            {
                path.CheckAccessibility();
            }
        }
    }
}
