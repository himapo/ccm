﻿using System;
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

        public Dungeon()
        {
            //Drawable = true;
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

            MapCollisionInfo.Color = Color.LightBlue;

            CollisionManager.Add(MapCollisionInfo);
        }

        public Vector3 GetRandomRespawnPoint()
        {
            var rectangles = DungeonMap.GetRoomRectangles().ToList();

            var roomNo = GameRand.Instance.Next(rectangles.Count);

            var rect = rectangles[roomNo];

            // 端の1マスは選ばない
            var x = GameRand.Instance.Next(rect.X + 1, rect.X + rect.Width - 1);
            var z = GameRand.Instance.Next(rect.Y + 1, rect.Y + rect.Height - 1);

            return new Vector3(-0.5f + x, 5, -0.5f + z) * GameProperty.CUBE_WIDTH;
        }
    }
}
