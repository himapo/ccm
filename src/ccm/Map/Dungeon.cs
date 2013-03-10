using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using ccm.DungeonLogic;
using ccm.System;

namespace ccm.Map
{
    /// <summary>
    /// キューブの集合によるマップ部分だけでなく、
    /// 遠景や特殊オブジェクトもすべて含めたダンジョン全体を表す
    /// </summary>
    public class Dungeon
    {
        IModel CubeModel;

        DungeonMap DungeonMap = new DungeonMap() { Rand = GameRand.Instance };

        List<AffineTransform> CubeTransforms = new List<AffineTransform>();

        bool MapUpdated = false;

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
            drawer.DrawMapCube(CubeModel, MapUpdated, CubeTransforms);
            MapUpdated = false;
        }
    }
}
