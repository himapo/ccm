using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ccm.CameraOld;

namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class MapManager : MyGameComponent
    {
        static MapManager instance;

        MapCubeModel instancedModel;
        List<MapCubeInstance> instances;
        Matrix[] instanceTransforms;

        MapCubeRenderParameter renderParam;

        HashSet<HimaLib.Math.Vector3> cubePosSet;

        public CameraLabel CameraLabel { get; set; }

        public static void CreateInstance(Game game)
        {
            instance = new MapManager(game);
        }

        public static MapManager GetInstance()
        {
            return instance;
        }

        MapManager(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.MAP;
            DrawOrder = (int)DrawOrderLabel.MAP;

            instancedModel = new MapCubeModel();
            instances = new List<MapCubeInstance>();

            renderParam = new MapCubeRenderParameter();

            cubePosSet = new HashSet<HimaLib.Math.Vector3>();

            CameraLabel = CameraLabel.Game;
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。

            base.Initialize();
        }

        protected override void LoadContent()
        {
            instancedModel.Load("Model/cube000");
        }        

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: ここにアップデートのコードを追加します。

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (instances.Count == 0)
                return;

            SetupInstanceTransforms();
            SetupRenderParameter();

            RenderManager.GetInstance().Register(renderParam);
        }

        void SetupInstanceTransforms()
        {
            Array.Resize(ref instanceTransforms, instances.Count);

            for (var i = 0; i < instances.Count; ++i)
            {
                instanceTransforms[i] = instances[i].Transform;
            }
        }

        void SetupInstanceTransformsWithFrustumCulling()
        {
            var notCulled = new List<int>();

            for (var i = 0; i < instances.Count; ++i)
            {
                if (!FrustumCulling.GetInstance().IsCulled(CameraLabel.Game, instancedModel.Model, instances[i].Transform))
                {
                    notCulled.Add(i);
                }
            }

            Array.Resize(ref instanceTransforms, notCulled.Count);

            for (var i = 0; i < notCulled.Count; ++i)
            {
                instanceTransforms[i] = instances[notCulled[i]].Transform;
            }
        }

        void SetupRenderParameter()
        {
            renderParam.cameraLabel = CameraLabel;
            renderParam.model = instancedModel.Model;
            renderParam.renderer = RendererLabel.MapCube;
            renderParam.world = Matrix.Identity;
            renderParam.cullEnable = false;

            renderParam.WorldMatrices = instanceTransforms;
            renderParam.ModelBones = instancedModel.ModelBones;
            renderParam.DirectionalLight0 = GetService<ILightService>().Get(LightAttribute.StageMain);
            renderParam.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);
        }

        public void Generate()
        {
            //var seed = GameProperty.gameRand.Next();
            //DebugUtil.PrintLine("seed: {0}", seed);
            //GameProperty.gameRand.Init(seed);

            cubePosSet.Clear();

            var cubePosList = MapGenerator.Instance.Generate().GetCubePosList();

            foreach (var pos in cubePosList)
            {
                if (!cubePosSet.Contains(pos))
                {
                    AddCube(pos.X, pos.Y, pos.Z);
                    cubePosSet.Add(pos);
                }
            }
        }

        void AddCube(float x, float y, float z)
        {
            var cube = new MapCubeInstance(Game);
            cube.Transform = Matrix.CreateScale(1.0f) * Matrix.CreateTranslation(x, y, z);
            instances.Add(cube);
        }

        public void Clear()
        {
            instances.Clear();
        }
    }
}
