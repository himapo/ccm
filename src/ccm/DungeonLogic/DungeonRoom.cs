using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    class DungeonRoom
    {
        public enum Side
        {
            Left,
            Right,
            Top,
            Bottom,
        }

        public const int WIDTH_MIN = 32;
        public const int WIDTH_MAX = 112;

        /// <summary>
        /// ブロックの外周から部屋の外郭までの最低間隔
        /// </summary>
        public const int MARGIN_MIN = 8;

        /// <summary>
        /// 出入口の角からの最低距離
        /// </summary>
        public const int PORTAL_CORNER_MARGIN = 5;

        /// <summary>
        /// 部屋の左上隅のきゅーぶ座標(x,z)
        /// きゅーぶ座標とは、きゅーぶ1個分を1単位とする座標のこと
        /// </summary>
        public Point LeftTopCoord { get; set; }

        /// <summary>
        /// 部屋のきゅーぶ幅(x,z)
        /// </summary>
        public Point Width { get; set; }

        /// <summary>
        /// 部屋の左上隅のきゅーぶY座標
        /// </summary>
        public int Height { get; set; }

        public List<DungeonPortal> Portals { get; set; }

        public bool Accessible { get; set; }

        public DungeonRoom(Point leftTop, Point width, int height)
        {
            LeftTopCoord = leftTop;
            Width = width;
            Height = height;

            Portals = new List<DungeonPortal>();

            Accessible = false;
        }

        public List<Vector3> GetCubePosList()
        {
            var result = new List<Vector3>();

            for (var x = LeftTopCoord.X; x < LeftTopCoord.X + Width.X; ++x)
            {
                for (var z = LeftTopCoord.Y; z < LeftTopCoord.Y + Width.Y; ++z)
                {
                    result.Add(new Vector3(GameProperty.CUBE_WIDTH * x, 0, GameProperty.CUBE_WIDTH * z));
                }
            }

            return result;
        }

        /// <summary>
        /// 部屋の出入口をランダムに決めて返す
        /// </summary>
        /// <returns></returns>
        public Point GetRandomPortalPoint(Side side)
        {
            var result = new Point();

            if (side == Side.Left || side == Side.Right)
            {
                if (side == Side.Left)
                {
                    result.X = LeftTopCoord.X - 1;
                }
                else if (side == Side.Right)
                {
                    result.X = LeftTopCoord.X + Width.X;
                }

                result.Y = GameProperty.gameRand.Next(Width.Y - PORTAL_CORNER_MARGIN * 2) + PORTAL_CORNER_MARGIN + LeftTopCoord.Y;
            }
            else if (side == Side.Top || side == Side.Bottom)
            {
                if (side == Side.Top)
                {
                    result.Y = LeftTopCoord.Y - 1;
                }
                else if (side == Side.Bottom)
                {
                    result.Y = LeftTopCoord.Y + Width.Y;
                }

                result.X = GameProperty.gameRand.Next(Width.X - PORTAL_CORNER_MARGIN * 2) + PORTAL_CORNER_MARGIN + LeftTopCoord.X;
            }

            return result;
        }

        public void CheckAccessibility()
        {
            if (Accessible)
                return;

            Accessible = true;

            foreach (var portal in Portals)
            {
                portal.CheckAccessibility();
            }
        }
    }
}
