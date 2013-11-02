using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using ccm.Game;

namespace ccm.DungeonLogic
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

        public HimaLib.Math.IRand Rand { get; set; }

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

        public Rectangle GetRectangle()
        {
            return new Rectangle(LeftTopCoord.X, LeftTopCoord.Y, Width.X, Width.Y);
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

                result.Y = Rand.Next(Width.Y - PORTAL_CORNER_MARGIN * 2) + PORTAL_CORNER_MARGIN + LeftTopCoord.Y;
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

                result.X = Rand.Next(Width.X - PORTAL_CORNER_MARGIN * 2) + PORTAL_CORNER_MARGIN + LeftTopCoord.X;
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

        /// <summary>
        /// 壁コリジョンを生成するための部屋の縁を求める
        /// 縁は幅1の矩形の集合として返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rectangle> GetOutlines()
        {
            var result = new List<Rectangle>();

            var leftX = LeftTopCoord.X - 1;
            var rightX = LeftTopCoord.X + Width.X;
            var topY = LeftTopCoord.Y - 1;
            var bottomY = LeftTopCoord.Y + Width.Y;

            // 部屋の各辺にポータルがあるか
            // left, right, top bottom の順
            bool[] edgeHasPortal = { false, false, false, false };

            foreach (var portal in Portals)
            {
                if (portal.ConnectedPaths.Count == 0)
                {
                    continue;
                }

                var pathWidth = portal.ConnectedPaths[0].Width;

                if (portal.Position.X == leftX || portal.Position.X == rightX)
                {
                    // ポータルが部屋の左右にある場合

                    if (portal.Position.X == leftX)
                    {
                        edgeHasPortal[0] = true;
                    }
                    else if (portal.Position.X == rightX)
                    {
                        edgeHasPortal[1] = true;
                    }

                    var outline0 = new Rectangle(
                        portal.Position.X,
                        LeftTopCoord.Y,
                        1,
                        portal.Position.Y - LeftTopCoord.Y - pathWidth / 2);
                    result.Add(outline0);
                    result.Add(new Rectangle(
                        portal.Position.X,
                        portal.Position.Y + 1 + pathWidth / 2,
                        1,
                        Width.Y - outline0.Height - pathWidth));
                }
                else if (portal.Position.Y == topY || portal.Position.Y == bottomY)
                {
                    // ポータルが部屋の上下にある場合

                    if (portal.Position.Y == topY)
                    {
                        edgeHasPortal[2] = true;
                    }
                    else if (portal.Position.Y == bottomY)
                    {
                        edgeHasPortal[3] = true;
                    }

                    var outline0 = new Rectangle(
                        LeftTopCoord.X,
                        portal.Position.Y,
                        portal.Position.X - LeftTopCoord.X - pathWidth / 2,
                        1);
                    result.Add(outline0);
                    result.Add(new Rectangle(
                        portal.Position.X + 1 + pathWidth / 2,
                        portal.Position.Y,
                        Width.X - outline0.Width - pathWidth,
                        1));
                }
            }

            // ポータルがない辺の縁を生成
            
            // 左辺
            if (!edgeHasPortal[0])
            {
                result.Add(new Rectangle(leftX, topY + 1, 1, Width.Y));
            }

            // 右辺
            if (!edgeHasPortal[1])
            {
                result.Add(new Rectangle(leftX + Width.X + 1, topY + 1, 1, Width.Y));
            }

            // 上辺
            if (!edgeHasPortal[2])
            {
                result.Add(new Rectangle(leftX + 1, topY, Width.X, 1));
            }

            // 下辺
            if (!edgeHasPortal[3])
            {
                result.Add(new Rectangle(leftX + 1, topY + Width.Y + 1, Width.X, 1));
            }

            return result;
        }
    }
}
