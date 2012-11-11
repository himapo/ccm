﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    class DungeonPath
    {
        public int Width { get; set; }
        public bool IsHorizontal { get; set; }

        public DungeonPortal[] Portals { get; set; }

        public bool Accessible { get; set; }
        
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

            Width = GameProperty.gameRand.Next(4) * 2 + 3;  // 3, 5, 7, 9
            
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

        int GetDevideNum(Point length)
        {
            var devideMax = 
                IsHorizontal 
                ? Math.Min(length.X + 1, length.Y + 2) 
                : Math.Min(length.Y + 1, length.X + 2);

            // 主方向をこの数だけ分割する
            // 水平通路で分割数2なら、水平、垂直、水平と歩くと端まで着く
            // 垂直通路で分割数3なら、垂直、水平、垂直、水平、垂直と歩くと端まで着く
            var devideNum = GameProperty.gameRand.Next(2, Math.Min(4, devideMax));

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
                var i = SelectRandomIndexExclude(result, 1, GameProperty.gameRand);
                if (i == -1)
                    break;

                // 長さaとbに分割（それぞれ1以上）
                var a = GameProperty.gameRand.Next(1, result[i]);
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
        int SelectRandomIndexExclude(List<int> list, int exclude, IRand rand)
        {
            var indices = new List<int>();
            for (var i = 0; i < list.Count; ++i)
            {
                if (list[i] != exclude)
                    indices.Add(i);
            }

            if (indices.Count == 0)
                return -1;

            return indices[rand.Next(indices.Count)];
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
    }
}
