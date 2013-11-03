using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using ccm.Game;

namespace ccm.DungeonLogic
{
    class DungeonPath
    {
        public int Width { get; set; }

        public bool IsHorizontal { get; set; }

        public DungeonPortal[] Portals { get; set; }

        public bool Accessible { get; set; }

        public HimaLib.Math.IRand Rand { get; set; }

        List<int> steps;

        public DungeonPath()
        {
            Width = 1;

            IsHorizontal = true;

            Portals = new DungeonPortal[2];

            Accessible = false;

            steps = new List<int>();
        }

        /// <summary>
        /// 通路の中間部を生成
        /// </summary>
        public void GenerateSteps()
        {
            var length = new Point();
            length.X = Math.Abs(Portals[0].Position.X - Portals[1].Position.X);
            length.Y = Math.Abs(Portals[0].Position.Y - Portals[1].Position.Y);

            Width = Rand.Next(4) * 2 + 3;  // 3, 5, 7, 9
            
            var minLengthFromRoom = Width / 2;  // 部屋から最初の曲がり角までの最低歩数

            if (IsRoomPath())
            {
                // 最初の曲がり角までの最低歩数*2を引いておく
                if (IsHorizontal)
                {
                    length.X -= minLengthFromRoom * 2;
                }
                else
                {
                    length.Y -= minLengthFromRoom * 2;
                }
            }

            var devideNum = GetDevideNum(length);

            // 水平・垂直方向別の次に曲がるまでに進む歩数リスト
            var stepsByDirection = new List<int>[2];

            if (IsHorizontal)
            {
                if (length.Y == 0)
                    devideNum = 1;

                // 水平
                stepsByDirection[0] = DevideLine(length.X, devideNum);
                // 垂直
                stepsByDirection[1] = DevideLine(length.Y, stepsByDirection[0].Count - 1);
            }
            else
            {
                if (length.X == 0)
                    devideNum = 1;

                // 垂直(最初の曲がり角までの最低歩数*2を引いておく)
                stepsByDirection[0] = DevideLine(length.Y, devideNum);
                // 水平
                stepsByDirection[1] = DevideLine(length.X, stepsByDirection[0].Count - 1);
            }

            if (IsRoomPath())
            {
                // 最初と最後の直線に最低歩数を足して距離を確保
                stepsByDirection[0][0] += minLengthFromRoom;
                stepsByDirection[0][stepsByDirection[0].Count - 1] += minLengthFromRoom;
            }

            // 進む歩数を交互に選んで一つのリストにマージする
            // 通路自体が水平方向の場合は、水平に4歩、垂直に3歩、水平に6歩・・・
            // 通路自体が垂直方向の場合は、垂直に4歩、水平に3歩、垂直に6歩・・・
            // というイメージ
            for (var i = 0; i < stepsByDirection[1].Count; ++i)
            {
                steps.Add(stepsByDirection[0][i]);
                steps.Add(stepsByDirection[1][i]);
            }
            steps.Add(stepsByDirection[0][stepsByDirection[0].Count - 1]);
        }

        /// <summary>
        /// 部屋につながった通路か？
        /// </summary>
        /// <returns></returns>
        bool IsRoomPath()
        {
            return Portals[0].ConnectedRoom != null || Portals[1].ConnectedRoom != null;
        }

        /// <summary>
        /// 曲がり角の数を決定する
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        int GetDevideNum(Point length)
        {
            var devideMax = 
                IsHorizontal 
                ? Math.Min(length.X + 1, length.Y + 2) 
                : Math.Min(length.Y + 1, length.X + 2);

            // 主方向をこの数だけ分割する
            // 水平通路で分割数2なら、水平、垂直、水平と歩くと端まで着く
            // 垂直通路で分割数3なら、垂直、水平、垂直、水平、垂直と歩くと端まで着く
            var devideNum = Rand.Next(2, Math.Min(4, devideMax));

            return devideNum;
        }

        /// <summary>
        /// 長さlengthの線分をcount個の線分にランダム分割
        /// </summary>
        List<int> DevideLine(int length, int count)
        {
            var result = new List<int>();

            // 分割不能
            if (length < count || count <= 0)
                return result;

            result.Add(length);

            if (length <= 1)
                return result;

            while (result.Count < count)
            {
                // 分割する線分を選ぶ（長さ1のものは選ばない）
                var i = SelectRandomIndexExclude(result, 1);
                if (i == -1)
                    break;

                // 長さaとbに分割（それぞれ1以上）
                var a = Rand.Next(1, result[i]);
                var b = result[i] - a;

                // リストを入れ替え
                result.Insert(i + 1, a);
                result.Insert(i + 2, b);
                result.RemoveAt(i);
            }

            return result;
        }

        /// <summary>
        /// exclude以外のものを一つ選んでインデックスを返す
        /// 選べない場合は-1を返す
        /// </summary>
        int SelectRandomIndexExclude(List<int> list, int exclude)
        {
            var indices = new List<int>();
            for (var i = 0; i < list.Count; ++i)
            {
                if (list[i] != exclude)
                    indices.Add(i);
            }

            if (indices.Count == 0)
                return -1;

            return indices[Rand.Next(indices.Count)];
        }

        public void CheckAccessibility()
        {
            if (Accessible)
                return;

            Accessible = true;

            Portals[0].CheckAccessibility();
            Portals[1].CheckAccessibility();
        }

        /// <summary>
        /// 行き止まりの方のポータルを取得
        /// </summary>
        /// <returns></returns>
        public DungeonPortal GetTerminalPortal()
        {
            for (var i = 0; i < 2; ++i)
            {
                if (Portals[i].ConnectedRoom != null)
                {
                    continue;
                }
                if (Portals[i].ConnectedPaths.Count == 2)
                {
                    continue;
                }

                return Portals[i];
            }

            return null;
        }

        public List<Vector3> GetCubePosList()
        {
            // 線分一つに太さを与えて矩形化し、矩形を構成するきゅーぶリストを作る
            // それをすべての線分に対してくり返す
            // 曲がり角の部分は重複するが、受け取る側で重複は排除する

            var result = new List<Vector3>();

            // 現在地点
            var position = new int[2] { Portals[0].Position.X, Portals[0].Position.Y };

            // 進む方向（水平か垂直か）
            var direction = IsHorizontal ? 0 : 1;

            // 1回に進む歩数（正か負か）
            var stepValue = new int[2] { Math.Sign(Portals[1].Position.X - Portals[0].Position.X), Math.Sign(Portals[1].Position.Y - Portals[0].Position.Y) };

            for (var i = 0; i < steps.Count; ++i)
            {
                var start = new Point(position[0], position[1]);

                position[direction] += stepValue[direction] * steps[i];

                var end = new Point(position[0], position[1]);

                var rect = CalcStepRect(start, end);

                // 矩形を構成するきゅーぶをリストに追加
                for (var x = rect.Left; x < rect.Right; ++x)
                {
                    for (var y = rect.Top; y < rect.Bottom; ++y)
                    {
                        result.Add(new Vector3(GameProperty.CUBE_WIDTH * x, 0, GameProperty.CUBE_WIDTH * y));
                    }
                }

                direction = 1 - direction;
            }

            return result;
        }

        /// <summary>
        /// 通路の曲がり角間の線分一つの矩形（きゅーぶ単位）を返す
        /// </summary>
        Rectangle CalcStepRect(Point start, Point end)
        {
            // 通路幅の半分だけ前後左右を伸ばす
            var additional = Width / 2;

            var result = new Rectangle(
                Math.Min(start.X, end.X) - additional, 
                Math.Min(start.Y, end.Y) - additional,
                Math.Abs(end.X - start.X) + 1 + additional * 2, 
                Math.Abs(end.Y - start.Y) + 1 + additional * 2);

            return result;
        }

        /// <summary>
        /// ポータル部分を除いた通路部分の矩形（きゅーぶ単位）リストを返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rectangle> GetRectangles()
        {
            var result = new List<Rectangle>();

            // 現在地点
            var position = new int[2] { Portals[0].Position.X, Portals[0].Position.Y };

            // 進む方向（水平か垂直か）
            var direction = IsHorizontal ? 0 : 1;

            // 1回に進む歩数（正か負か）
            var stepValue = new int[2]
            { 
                Math.Sign(Portals[1].Position.X - Portals[0].Position.X),
                Math.Sign(Portals[1].Position.Y - Portals[0].Position.Y)
            };

            for (var i = 0; i < steps.Count; ++i)
            {
                var start = new Point(position[0], position[1]);

                position[direction] += stepValue[direction] * steps[i];

                var end = new Point(position[0], position[1]);

                // 曲がり角2つの間に隙間があるときは間の通路を作る
                if (steps[i] > Width)
                {
                    // ポータルと曲がり角を除いた長方形
                    result.Add(CalcStepRect2(start, end, direction == 0));   
                }

                // 曲がり角部分の正方形
                if (i < steps.Count - 1)
                {
                    result.Add(new Rectangle(
                        end.X - Width / 2,
                        end.Y - Width / 2,
                        Width,
                        Width));
                }

                direction = 1 - direction;
            }

            return result;
        }

        Rectangle CalcStepRect2(Point start, Point end, bool horizontal)
        {
            // 通路幅の半分だけ幅を広げる
            // 幅3の通路なら1マス広げる
            var additional = Width / 2;

            // 曲がり角部分は削る
            // 幅3の通路なら2マス削る
            var cut = additional + 1;

            // 水平通路ならx方向を削る
            // 垂直通路ならy方向を削る

            var x = Math.Min(start.X, end.X) + (horizontal ? cut : -additional);
            var y = Math.Min(start.Y, end.Y) + (horizontal ? -additional : cut);
            
            var w = Math.Abs(end.X - start.X) + 1;
            w = horizontal ? (w - cut * 2) : Width;
            var h = Math.Abs(end.Y - start.Y) + 1;
            h = horizontal ? Width : (h - cut * 2);

            var result = new Rectangle(x, y, w, h);

            return result;
        }

        /// <summary>
        /// 壁コリジョン生成のための通路の縁を計算する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Rectangle> GetOutlines()
        {
            var result = new List<Rectangle>();

            // 現在地点
            var position = new int[2] { Portals[0].Position.X, Portals[0].Position.Y };

            // 進む方向（水平か垂直か）
            var direction = IsHorizontal ? 0 : 1;

            // 1回に進む歩数（正か負か）
            var stepValue = new int[2]
            { 
                Math.Sign(Portals[1].Position.X - Portals[0].Position.X),
                Math.Sign(Portals[1].Position.Y - Portals[0].Position.Y)
            };

            for (var i = 0; i < steps.Count; ++i)
            {
                var start = new Point(position[0], position[1]);

                position[direction] += stepValue[direction] * steps[i];

                var end = new Point(position[0], position[1]);

                var outlines = new Rectangle[2];

                // まずstepの長さの壁を作る

                if (direction == 0)
                {
                    // 上側の壁
                    outlines[0].X = MathUtil.Min(start.X, end.X);
                    outlines[0].Y = start.Y - (Width / 2) - 1;
                    outlines[0].Width = steps[i];
                    outlines[0].Height = 1;

                    // 下側の壁
                    outlines[1].X = MathUtil.Min(start.X, end.X);
                    outlines[1].Y = start.Y + (Width / 2) + 1;
                    outlines[1].Width = steps[i];
                    outlines[1].Height = 1;
                }
                else if (direction == 1)
                {
                    // 左側の壁
                    outlines[0].X = start.X - (Width / 2) - 1;
                    outlines[0].Y = MathUtil.Min(start.Y, end.Y);
                    outlines[0].Width = 1;
                    outlines[0].Height = steps[i];

                    // 右側の壁
                    outlines[1].X = start.X + (Width / 2) + 1;
                    outlines[1].Y = MathUtil.Min(start.Y, end.Y);
                    outlines[1].Width = 1;
                    outlines[1].Height = steps[i];
                }

                // 曲がり角の内側の壁を削って外側の壁を伸ばす
                // 曲がり角の向きと進行方向によって8パターンある

                // 削る量
                var cut = (Width / 2) + 1;
                // 伸ばす量
                var add = (Width / 2) + 1;

                // 最初の通路でなければ始点をいじる
                if (i > 0)
                {
                    // 水平通路で
                    if (direction == 0)
                    {
                        // 前の通路が下から上で
                        if (stepValue[1 - direction] < 0)
                        {
                            // この通路が右から左なら
                            if (stepValue[direction] < 0)
                            {
                                // 下側の壁の右を削る
                                // 上側の壁の右を伸ばす
                            }
                            // この通路が左から右なら
                            else
                            {
                                // 下側の壁の左を削る
                                outlines[1].X += cut;
                                // 上側の壁の左を伸ばす
                                outlines[0].X -= add;
                            }

                            outlines[0].Width += add;
                            outlines[1].Width -= cut;
                        }
                        // 前の通路が上から下で
                        else
                        {
                            // この通路が右から左なら
                            if (stepValue[direction] < 0)
                            {
                                // 上側の壁の右を削る
                                // 下側の壁の右を伸ばす
                            }
                            // この通路が左から右なら
                            else
                            {
                                // 上側の壁の左を削る
                                outlines[0].X += cut;
                                // 下側の壁の左を伸ばす
                                outlines[1].X -= add;
                            }

                            outlines[0].Width -= cut;
                            outlines[1].Width += add;
                        }
                    }
                    // 垂直通路で
                    else if (direction == 1)
                    {
                        // 前の通路が右から左で
                        if (stepValue[1 - direction] < 0)
                        {
                            // この通路が下から上なら
                            if (stepValue[direction] < 0)
                            {
                                // 右側の壁の下を削る
                                // 左側の壁の下を伸ばす
                            }
                            // この通路が上から下なら
                            else
                            {
                                // 右側の壁の上を削る
                                outlines[1].Y += cut;
                                // 左側の壁の上を伸ばす
                                outlines[0].Y -= add;
                            }

                            outlines[0].Height += add;
                            outlines[1].Height -= cut;
                        }
                        // 前の通路が左から右で
                        else
                        {
                            // この通路が下から上なら
                            if (stepValue[direction] < 0)
                            {
                                // 左側の壁の下を削る
                                // 右側の壁の下を伸ばす
                            }
                            // この通路が上から下なら
                            else
                            {
                                // 左側の壁の上を削る
                                outlines[0].Y += cut;
                                // 右側の壁の上を伸ばす
                                outlines[1].Y -= add;
                            }

                            outlines[0].Height -= cut;
                            outlines[1].Height += add;
                        }
                    }
                }

                // 最後の通路でなければ終点をいじる
                if (i < steps.Count - 1)
                {
                    // 水平通路で
                    if (direction == 0)
                    {
                        // 次の通路が下から上で
                        if (stepValue[1 - direction] < 0)
                        {
                            // この通路が右から左なら
                            if (stepValue[direction] < 0)
                            {
                                // 上側の壁の左を削る
                                outlines[0].X += cut;
                                // 下側の壁の左を伸ばす
                                outlines[1].X -= add;
                            }
                            // この通路が左から右なら
                            else
                            {
                                // 上側の壁の右を削る
                                // 下側の壁の右を伸ばす
                            }

                            outlines[0].Width -= cut;
                            outlines[1].Width += add;
                        }
                        // 次の通路が上から下で
                        else
                        {
                            // この通路が右から左なら
                            if (stepValue[direction] < 0)
                            {
                                // 下側の壁の左を削る
                                outlines[1].X += cut;
                                // 上側の壁の左を伸ばす
                                outlines[0].X -= add;
                            }
                            // この通路が左から右なら
                            else
                            {
                                // 下側の壁の右を削る
                                // 上側の壁の右を伸ばす
                            }

                            outlines[0].Width += add;
                            outlines[1].Width -= cut;
                        }
                    }
                    // 垂直通路で
                    else if (direction == 1)
                    {
                        // 次の通路が右から左で
                        if (stepValue[1 - direction] < 0)
                        {
                            // この通路が下から上なら
                            if (stepValue[direction] < 0)
                            {
                                // 左側の壁の上を削る
                                outlines[0].Y += cut;
                                // 右側の壁の上を伸ばす
                                outlines[1].Y -= add;
                            }
                            // この通路が上から下なら
                            else
                            {
                                // 左側の壁の下を削る
                                // 右側の壁の下を伸ばす
                            }

                            outlines[0].Height -= cut;
                            outlines[1].Height += add;
                        }
                        // 次の通路が左から右で
                        else
                        {
                            // この通路が下から上なら
                            if (stepValue[direction] < 0)
                            {
                                // 右側の壁の上を削る
                                outlines[1].Y += cut;
                                // 左側の壁の上の伸ばす
                                outlines[0].Y -= add;
                            }
                            // この通路が上から下なら
                            else
                            {
                                // 右側の壁の下を削る
                                // 左側の壁の下を伸ばす
                            }

                            outlines[0].Height += add;
                            outlines[1].Height -= cut;
                        }
                    }
                }

                result.AddRange(outlines);

                direction = 1 - direction;
            }

            return result;
        }
    }
}
