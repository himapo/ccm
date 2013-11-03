using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace ccm.DungeonLogic
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

        public Rectangle GetRectangle()
        {
            if (ConnectedPaths.Count == 0)
            {
                return new Rectangle(0, 0, 0, 0);
            }

            // つながっている通路で一番広い幅
            var maxWidth = ConnectedPaths.Max((path) => { return path.Width; });

            // ポータルはその幅を一辺とする正方形
            return new Rectangle(
                Position.X - maxWidth / 2,
                Position.Y - maxWidth / 2,
                maxWidth,
                maxWidth);
        }

        /// <summary>
        /// 袋小路部分の縁を計算する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rectangle> GetOutlines()
        {
            var result = new List<Rectangle>();



            return result;
        }
    }
}
