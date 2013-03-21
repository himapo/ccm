using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using ccm.Util;

namespace ccm.DungeonLogic
{
    public class DungeonMap
    {
        const int BLOCK_WIDTH = 128;
        const int BLOCK_NUM_X = 4;
        const int BLOCK_NUM_Z = 4;

        /// <summary>
        /// 袋小路の処置
        /// </summary>
        enum DeadEndDisposal
        {
            CONNECT,    // つなぐ
            REMOVE,     // 消す
            LEAVE,      // 残す
        }

        public HimaLib.Math.IRand Rand { get; set; }

        List<DungeonRoom> Rooms { get; set; }
        List<DungeonPath> Paths { get; set; }
        List<DungeonPortal> Portals { get; set; }

        DungeonRoom removedRoom;
        List<DungeonPath> removedPaths;
        List<DungeonPath> addedPaths;

        public DungeonMap()
        {
            Rooms = new List<DungeonRoom>();
            Paths = new List<DungeonPath>();
            Portals = new List<DungeonPortal>();

            removedPaths = new List<DungeonPath>();
            addedPaths = new List<DungeonPath>();
        }

        public void Generate()
        {
            Clear();
            GenerateRooms();
            GeneratePaths();
            ThinOutRooms();
        }

        public void Clear()
        {
            Rooms.Clear();
            Paths.Clear();
        }

        public List<Vector3> GetCubePosList()
        {
            var result = new List<Vector3>();

            foreach (var room in Rooms)
            {
                result.AddRange(room.GetCubePosList());
            }

            foreach (var path in Paths)
            {
                result.AddRange(path.GetCubePosList());
            }

            return result;
        }

        void GenerateRooms()
        {
            var blocks = ChooseBlocks();

            foreach (var block in blocks)
            {
                Rooms.Add(GenerateRoom(block));
            }
        }

        List<int> ChooseBlocks()
        {
            var result = new List<int>();

            var blockSource = new ShuffleList<int>(Rand);

            blockSource.AddRange(GeneralUtil.Range(16));
            //blockSource.Draw(GameProperty.gameRand.Next(5, 10));

            // 今は全ブロックをそのまま返す
            result.AddRange(blockSource.RemainList);

            return result;
        }

        DungeonRoom GenerateRoom(int block)
        {
            var blockCorner = CalcCornerPoint(block);

            var width_x = Rand.Next(DungeonRoom.WIDTH_MIN, DungeonRoom.WIDTH_MAX);
            var width_y = Rand.Next(DungeonRoom.WIDTH_MIN, DungeonRoom.WIDTH_MAX);

            var margin_x = Rand.Next(DungeonRoom.MARGIN_MIN, BLOCK_WIDTH - width_x - DungeonRoom.MARGIN_MIN);
            var margin_y = Rand.Next(DungeonRoom.MARGIN_MIN, BLOCK_WIDTH - width_y - DungeonRoom.MARGIN_MIN);

            var leftTop = new Point(blockCorner.X + margin_x, blockCorner.Y + margin_y);
            var width = new Point(width_x, width_y);

            return new DungeonRoom(leftTop, width, 0) { Rand = this.Rand };
        }

        Point CalcCornerPoint(int block)
        {
            var result = new Point();

            var x = -BLOCK_WIDTH * BLOCK_NUM_X / 2;
            var z = -BLOCK_WIDTH * BLOCK_NUM_Z / 2;

            x += BLOCK_WIDTH * (block % 4);
            z += BLOCK_WIDTH * (block / 4);

            result.X = x;
            result.Y = z;

            return result;
        }

        void GeneratePaths()
        {
            for (var z = 0; z < BLOCK_NUM_Z; ++z)
            {
                for (var x = 0; x < BLOCK_NUM_X; ++x)
                {
                    // 右端の部屋でなければ右方向の通路を作る
                    if (x < BLOCK_NUM_X - 1)
                    {
                        GenerateRoomPaths(
                            Rooms[z * BLOCK_NUM_X + x], Rooms[z * BLOCK_NUM_X + x + 1],
                            true);
                    }
                    // 下端の部屋でなければ下方向の通路を作る
                    if (z < BLOCK_NUM_Z - 1)
                    {
                        GenerateRoomPaths(
                            Rooms[z * BLOCK_NUM_X + x], Rooms[(z + 1) * BLOCK_NUM_X + x],
                            false);
                    }
                }
            }
        }

        /// <summary>
        /// 隣同士の部屋をつなぐ通路を生成
        /// </summary>
        void GenerateRoomPaths(DungeonRoom startRoom, DungeonRoom endRoom, bool isHorizontal)
        {
            var portals = new DungeonPortal[2];
            GenerateRoomPortals(
                startRoom, endRoom,
                isHorizontal,
                out portals[0], out portals[1]);

            Paths.Add(GeneratePath(portals[0], portals[1], isHorizontal));
            Portals.Add(portals[0]);
            Portals.Add(portals[1]);
        }

        /// <summary>
        /// 隣同士の部屋をつなぐためのポータルペアを生成
        /// </summary>
        void GenerateRoomPortals(
            DungeonRoom startRoom, DungeonRoom endRoom, 
            bool isHorizontal, 
            out DungeonPortal startPortal, out DungeonPortal endPortal)
        {
            startPortal = new DungeonPortal(
                startRoom.GetRandomPortalPoint(isHorizontal ? DungeonRoom.Side.Right : DungeonRoom.Side.Bottom));
            startPortal.ConnectedRoom = startRoom;
            startRoom.Portals.Add(startPortal);

            endPortal = new DungeonPortal(
                endRoom.GetRandomPortalPoint(isHorizontal ? DungeonRoom.Side.Left : DungeonRoom.Side.Top));
            endPortal.ConnectedRoom = endRoom;
            endRoom.Portals.Add(endPortal);
        }

        DungeonPath GeneratePath(DungeonPortal start, DungeonPortal end, bool isHorizontal)
        {
            var result = new DungeonPath() { Rand = this.Rand };

            start.ConnectedPaths.Add(result);
            result.Portals[0] = start;

            end.ConnectedPaths.Add(result);
            result.Portals[1] = end;

            result.IsHorizontal = isHorizontal;

            result.GenerateSteps();

            return result;
        }

        /// <summary>
        /// 部屋をいくつか間引く
        /// </summary>
        void ThinOutRooms()
        {
            var removeNum = Rand.Next(7, 12);

            for (var i = 0; i < removeNum; ++i)
            {
                removedRoom = null;
                removedPaths.Clear();
                addedPaths.Clear();

                ThinOutRoom();
                if (!IsValidMap())
                {
                    UndoRoom();
                    UndoPaths();
                    i--;
                }
            }
        }

        void ThinOutRoom()
        {
            RemoveRoom(Rooms[Rand.Next(Rooms.Count)]);

            ProcessDeadEnd(removedRoom);
        }

        void ProcessDeadEnd(DungeonRoom room)
        {
            var deadends = new List<DungeonPath>();

            UpdateAccessibility();

            foreach (var portal in room.Portals)
            {
                if (portal.ConnectedPaths.Count == 0)
                    continue;

                var path = portal.ConnectedPaths[0];

                if (path.Accessible)
                {
                    // 孤立していない通路をリストアップ
                    deadends.Add(path);
                }
                else
                {
                    // 孤立している通路は消す
                    RemovePath(path);
                }
            }

            var selector = new RandomSelector<DeadEndDisposal>(Rand);
            selector.Add(40, DeadEndDisposal.CONNECT);
            selector.Add(55, DeadEndDisposal.REMOVE);
            selector.Add(5, DeadEndDisposal.LEAVE);

            while (deadends.Count > 0)
            {
                var disposal = selector.Get();

                if (disposal == DeadEndDisposal.CONNECT && deadends.Count >= 2)
                {
                    var another = Rand.Next(1, deadends.Count);

                    var portals = new DungeonPortal[2]{
                        deadends[0].GetTerminalPortal(),
                        deadends[another].GetTerminalPortal()
                    };

                    AddPath(GeneratePath(
                        portals[0],
                        portals[1],
                        IsHorizonalPortals(portals[0], portals[1])
                        ));

                    deadends.RemoveAt(another);
                    deadends.RemoveAt(0);
                }
                else if (disposal == DeadEndDisposal.REMOVE)
                {
                    RemovePath(deadends[0]);
                    deadends.RemoveAt(0);
                }
                else if (disposal == DeadEndDisposal.LEAVE)
                {
                    deadends.RemoveAt(0);
                }
            }

        }

        void RemoveRoom(DungeonRoom room)
        {
            removedRoom = room;

            Rooms.Remove(room);

            foreach (var portal in room.Portals)
            {
                portal.ConnectedRoom = null;
            }
        }

        void RemovePath(DungeonPath path)
        {
            var removeList = new List<DungeonPath>();

            // つながっている先を再帰的に消したいので、まずチェックをつける
            CheckRemovePath(path, removeList);

            // 一気に消す
            foreach (var p in removeList)
            {
                removedPaths.Add(p);
                Paths.Remove(p);

                p.Portals[0].ConnectedPaths.Remove(p);
                p.Portals[1].ConnectedPaths.Remove(p);
            }
        }

        void CheckRemovePath(DungeonPath path, List<DungeonPath> removeList)
        {
            if (removeList.Contains(path))
                return;

            removeList.Add(path);

            foreach (var connectedPath in path.Portals[0].ConnectedPaths)
            {
                CheckRemovePath(connectedPath, removeList);
            }
            foreach (var connectedPath in path.Portals[1].ConnectedPaths)
            {
                CheckRemovePath(connectedPath, removeList);
            }
        }

        void AddPath(DungeonPath path)
        {
            addedPaths.Add(path);
            Paths.Add(path);

            if (!path.Portals[0].ConnectedPaths.Contains(path))
            {
                path.Portals[0].ConnectedPaths.Add(path);
            }
            if (!path.Portals[1].ConnectedPaths.Contains(path))
            {
                path.Portals[1].ConnectedPaths.Add(path);
            }
        }

        void UndoRoom()
        {
            Rooms.Add(removedRoom);
            foreach (var portal in removedRoom.Portals)
            {
                portal.ConnectedRoom = removedRoom;
            }
        }

        void UndoPaths()
        {
            foreach (var path in addedPaths)
            {
                Paths.Remove(path);
                path.Portals[0].ConnectedPaths.Remove(path);
                path.Portals[1].ConnectedPaths.Remove(path);
            }
            foreach (var path in removedPaths)
            {
                Paths.Add(path);
                path.Portals[0].ConnectedPaths.Add(path);
                path.Portals[1].ConnectedPaths.Add(path);
            }
        }

        /// <summary>
        /// すべてつながっているマップか？
        /// </summary>
        bool IsValidMap()
        {
            UpdateAccessibility();

            foreach (var room in Rooms)
            {
                if (!room.Accessible)
                    return false;
            }

            return true;
        }

        void UpdateAccessibility()
        {
            foreach (var room in Rooms)
            {
                room.Accessible = false;
            }

            foreach (var path in Paths)
            {
                path.Accessible = false;
            }

            foreach (var portal in Portals)
            {
                portal.Accessible = false;
            }

            Rooms[0].CheckAccessibility();
        }

        bool IsHorizonalPortals(DungeonPortal start, DungeonPortal end)
        {
            var length = new Point();
            length.X = Math.Abs(start.Position.X - end.Position.X);
            length.Y = Math.Abs(start.Position.Y - end.Position.Y);

            return length.X >= length.Y;
        }
        
    }
}
