using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Debug;

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
        /// 壁コリジョン生成用の縁を計算する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rectangle> GetOutlines()
        {
            var result = new List<Rectangle>();

            // 部屋につながっている
            if (ConnectedRoom != null)
            {
                global::System.Diagnostics.Debug.Assert(ConnectedPaths.Count == 1);

                var pathWidthHalfPlusOne = ConnectedPaths[0].Width / 2 + 1;

                var connectedRoomSide = GetConnectedRoomSide();

                // 部屋の左辺
                if (connectedRoomSide == 0)
                {
                    result.Add(new Rectangle(
                        Position.X - pathWidthHalfPlusOne + 1,
                        Position.Y - pathWidthHalfPlusOne,
                        pathWidthHalfPlusOne,
                        1));
                    result.Add(new Rectangle(
                        Position.X - pathWidthHalfPlusOne + 1,
                        Position.Y + pathWidthHalfPlusOne,
                        pathWidthHalfPlusOne,
                        1));
                }
                // 部屋の右辺
                else if (connectedRoomSide == 1)
                {
                    result.Add(new Rectangle(
                        Position.X,
                        Position.Y - pathWidthHalfPlusOne,
                        pathWidthHalfPlusOne,
                        1));
                    result.Add(new Rectangle(
                        Position.X,
                        Position.Y + pathWidthHalfPlusOne,
                        pathWidthHalfPlusOne,
                        1));
                }
                // 部屋の上辺
                else if (connectedRoomSide == 2)
                {
                    result.Add(new Rectangle(
                        Position.X - pathWidthHalfPlusOne,
                        Position.Y - pathWidthHalfPlusOne + 1,
                        1,
                        pathWidthHalfPlusOne));
                    result.Add(new Rectangle(
                        Position.X + pathWidthHalfPlusOne,
                        Position.Y - pathWidthHalfPlusOne + 1,
                        1,
                        pathWidthHalfPlusOne));
                }
                // 部屋の下辺
                else if (connectedRoomSide == 3)
                {
                    result.Add(new Rectangle(
                        Position.X - pathWidthHalfPlusOne,
                        Position.Y,
                        1,
                        pathWidthHalfPlusOne));
                    result.Add(new Rectangle(
                        Position.X + pathWidthHalfPlusOne,
                        Position.Y,
                        1,
                        pathWidthHalfPlusOne));
                }
            }
            // 2本の通路につながっている
            else if (ConnectedPaths.Count == 2)
            {
                int[] pathWidth = 
                {
                    ConnectedPaths[0].Width,
                    ConnectedPaths[1].Width
                };
                int[] pathWidthHalf =
                {
                    pathWidth[0] / 2,
                    pathWidth[1] / 2
                };

                var maxWidth = MathUtil.Max(pathWidth[0], pathWidth[1]);
                var minWidth = MathUtil.Min(pathWidth[0], pathWidth[1]);

                // とりあえず全方位の壁を作る
                CreateAllOutlines(result, maxWidth, MathUtil.Max(pathWidthHalf[0], pathWidthHalf[1]));

                var removeList = new List<Rectangle>();

                for (var i = 0; i < 2; ++i)
                {
                    var connectedPathSide = GetConnectedPathSide(ConnectedPaths[i]);

                    removeList.Add(result[connectedPathSide]);

                    // 広い通路の方は何も追加せず
                    if (pathWidth[i] == maxWidth)
                    {
                    }
                    // 狭い通路の方は穴を開けて二分割した壁を追加
                    else if (maxWidth > minWidth && pathWidth[i] == minWidth)
                    {
                        if (connectedPathSide == 0 || connectedPathSide == 1)
                        {
                            var outline0 = new Rectangle(
                                result[connectedPathSide].X,
                                result[connectedPathSide].Y,
                                1,
                                (maxWidth - minWidth) / 2);
                            result.Add(outline0);
                            result.Add(new Rectangle(
                                outline0.X,
                                outline0.Y + outline0.Height + minWidth,
                                1,
                                (maxWidth - minWidth) / 2));
                        }
                        else if (connectedPathSide == 2 || connectedPathSide == 3)
                        {
                            var outline0 = new Rectangle(
                                result[connectedPathSide].X,
                                result[connectedPathSide].Y,
                                (maxWidth - minWidth) / 2,
                                1);
                            result.Add(outline0);
                            result.Add(new Rectangle(
                                outline0.X + outline0.Width + minWidth,
                                outline0.Y,
                                (maxWidth - minWidth) / 2,
                                1));
                        }
                    }
                }

                // 元の壁を消す
                removeList.ForEach((rect) => { result.Remove(rect); });
            }
            // 1本の通路につながっている（袋小路）
            else if (ConnectedPaths.Count == 1)
            {
                var pathWidth = ConnectedPaths[0].Width;
                var pathWidthHalf = pathWidth / 2;

                // とりあえず全方位の壁を作る
                CreateAllOutlines(result, pathWidth, pathWidthHalf);

                var connectedPathSide = GetConnectedPathSide(ConnectedPaths[0]);

                // 通路がある方の壁を消去
                result.RemoveAt(connectedPathSide);
            }

            return result;
        }

        bool IsConnectedToRoom()
        {
            return ConnectedRoom != null;
        }

        /// <summary>
        /// 部屋のどちら側にポータルがつながっているか
        /// </summary>
        /// <returns>-1:部屋につながってない 0:左 1:右 2:上 3:下</returns>
        int GetConnectedRoomSide()
        {
            if (!IsConnectedToRoom())
            {
                return -1;
            }

            if (Position.X == ConnectedRoom.LeftTopCoord.X - 1)
            {
                return 0;
            }

            if (Position.X == ConnectedRoom.LeftTopCoord.X + ConnectedRoom.Width.X)
            {
                return 1;
            }

            if (Position.Y == ConnectedRoom.LeftTopCoord.Y - 1)
            {
                return 2;
            }

            if (Position.Y == ConnectedRoom.LeftTopCoord.Y + ConnectedRoom.Width.Y)
            {
                return 3;
            }

            return -1;
        }

        /// <summary>
        /// ポータルのどちら側に通路がつながっているか
        /// </summary>
        /// <param name="path"></param>
        /// <returns>0:左 1:右 2:上 3:下</returns>
        int GetConnectedPathSide(DungeonPath path)
        {
            if (path.IsHorizontal)
            {
                if (path.Portals[0] == this)
                {
                    if (path.Portals[0].Position.X < path.Portals[1].Position.X)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else if (path.Portals[1] == this)
                {
                    if (path.Portals[0].Position.X < path.Portals[1].Position.X)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else
            {
                if (path.Portals[0] == this)
                {
                    if (path.Portals[0].Position.Y < path.Portals[1].Position.Y)
                    {
                        return 3;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else if (path.Portals[1] == this)
                {
                    if (path.Portals[0].Position.Y < path.Portals[1].Position.Y)
                    {
                        return 2;
                    }
                    else
                    {
                        return 3;
                    }
                }
            }

            return -1;
        }

        void CreateAllOutlines(List<Rectangle> result, int pathWidth, int pathWidthHalf)
        {
            // 左
            result.Add(new Rectangle(
                Position.X - pathWidthHalf - 1,
                Position.Y - pathWidthHalf,
                1,
                pathWidth));

            // 右
            result.Add(new Rectangle(
                Position.X + pathWidthHalf + 1,
                Position.Y - pathWidthHalf,
                1,
                pathWidth));

            // 上
            result.Add(new Rectangle(
                Position.X - pathWidthHalf,
                Position.Y - pathWidthHalf - 1,
                pathWidth,
                1));

            // 下
            result.Add(new Rectangle(
                Position.X - pathWidthHalf,
                Position.Y + pathWidthHalf + 1,
                pathWidth,
                1));
        }
    }
}
