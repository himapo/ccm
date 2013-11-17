using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Collision;
using ccm.DungeonLogic;
using ccm.System;
using ccm.Game;
using ccm.Collision;

namespace ccm.Map
{
    /// <summary>
    /// キューブの集合によるマップ部分だけでなく、
    /// 遠景や特殊オブジェクトもすべて含めたダンジョン全体を表す
    /// </summary>
    public class Dungeon
    {
        public bool Drawable { get; set; }

        public CollisionManager CollisionManager { get; set; }

        IModel CubeModel;

        DungeonMap DungeonMap = new DungeonMap() { Rand = GameRand.Instance };

        List<AffineTransform> CubeTransforms = new List<AffineTransform>();

        bool MapUpdated = false;

        MapCollisionInfo FloorCollisionInfo = new MapCollisionInfo();

        MapCollisionInfo WallCollisionInfo = new MapCollisionInfo();

        public Dungeon()
        {
            FloorCollisionInfo.Color = Color.LightBlue;
            FloorCollisionInfo.Group = () => (int)CollisionGroup.Floor;

            WallCollisionInfo.Color = Color.LightGreen;
            WallCollisionInfo.Group = () => (int)CollisionGroup.Wall;
        }

        public void InitModel()
        {
            CubeModel = ModelFactory.Instance.Create("cube000");
        }

        public void Generate()
        {
            DungeonMap.Generate();

            var cubePosList = DungeonMap.GetCubePosList();
            foreach (var pos in cubePosList)
            {
                CubeTransforms.Add(
                    new HimaLib.Math.AffineTransform(
                        HimaLib.Math.Vector3.One,
                        HimaLib.Math.Vector3.Zero,
                        new Vector3(pos.X, pos.Y - 1.5f, pos.Z)));
            }

            MapUpdated = true;
        }

        public void Draw(IDungeonDrawer drawer)
        {
            if (!Drawable)
            {
                return;
            }
            drawer.DrawMapCube(CubeModel, MapUpdated, CubeTransforms);
            MapUpdated = false;
        }

        public void GenerateFloorCollision()
        {
            var rectangles = DungeonMap.GetRoomRectangles().Concat(DungeonMap.GetPathRectangles()).Concat(DungeonMap.GetPortalRectangles());

            foreach (var rect in rectangles)
            {
                var corner = new Vector3(
                    -0.5f + rect.X,
                    -1.0f,
                    -0.5f + rect.Y) * GameProperty.CUBE_WIDTH;

                var width = new Vector3(
                    rect.Width,
                    1.0f,
                    rect.Height) * GameProperty.CUBE_WIDTH;

                FloorCollisionInfo.AddAABB(corner, width);
            }

            CollisionManager.Add(FloorCollisionInfo);
        }

        public void GenerateWallCollision()
        {
            var outlines = DungeonMap.GetRoomOutlines().Concat(DungeonMap.GetPathOutlines()).Concat(DungeonMap.GetPortalOutlines());

            foreach (var rect in outlines)
            {
                var corner = new Vector3(
                    -0.5f + rect.X,
                    -1.0f,
                    -0.5f + rect.Y) * GameProperty.CUBE_WIDTH;

                var width = new Vector3(
                    rect.Width,
                    2.0f,
                    rect.Height) * GameProperty.CUBE_WIDTH;

                WallCollisionInfo.AddAABB(corner, width);
            }

            CollisionManager.Add(WallCollisionInfo);
        }

        public Vector3 GetRandomRespawnPoint()
        {
            var rectangles = DungeonMap.GetRoomRectangles().ToList();

            var roomNo = GameRand.Instance.Next(rectangles.Count);

            var rect = rectangles[roomNo];

            // 端の1マスは選ばない
            var x = GameRand.Instance.Next(rect.X + 1, rect.X + rect.Width - 1);
            var z = GameRand.Instance.Next(rect.Y + 1, rect.Y + rect.Height - 1);

            return new Vector3(-0.5f + x, 0, -0.5f + z) * GameProperty.CUBE_WIDTH;
        }
    }
}
