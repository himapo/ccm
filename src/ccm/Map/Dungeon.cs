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

        MapCollisionInfo MapCollisionInfo = new MapCollisionInfo();

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

        public void InitCollision()
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

                MapCollisionInfo.AddAABB(corner, width);
            }

            CollisionManager.Add(MapCollisionInfo);
        }
    }
}
