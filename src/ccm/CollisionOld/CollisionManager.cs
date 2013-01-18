using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ccm.CameraOld;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class CollisionManager : MyGameComponent, ICollisionService
    {
        List<CollisionInfo> infoList;
        int IDCount;
        List<int> availableIDList;
        Dictionary<int, Dictionary<int, int>> collisionMatrix;

        // デバッグ表示用
        BasicEffect basicEffect;
        VertexPositionColor[] sphereVertices;
        short[] sphereIndices;
        VertexPositionColor[] cylinderVertices;
        short[] cylinderIndices;

        public CollisionManager(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.COLLISION;
            DrawOrder = (int)DrawOrderLabel.COLLISION;

            infoList = new List<CollisionInfo>();
            IDCount = 0;
            availableIDList = new List<int>();
            collisionMatrix = new Dictionary<int, Dictionary<int, int>>();

            // TODO: ここで子コンポーネントを作成します。

            game.Services.AddService(typeof(ICollisionService), this);
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            InitializeDebug();

            base.Initialize();
        }

        [Conditional("DEBUG")]
        void InitializeDebug()
        {
            basicEffect = new BasicEffect(Game.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 800.0f), Vector3.Zero, Vector3.Up);
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(GameProperty.fov),
                (float)GameProperty.resolutionWidth / GameProperty.resolutionHeight,
                10.0f,
                1000.0f);

            // 球の頂点初期化
            sphereVertices = new VertexPositionColor[12 * 5 + 2];

            for (var i = 0; i < 5; ++i)
            {
                var theta = MathHelper.ToRadians(30.0f * (i + 1));
                for (var j = 0; j < 12; ++j)
                {
                    var phi = MathHelper.ToRadians(30.0f * j);
                    sphereVertices[i * 12 + j].Position = new Vector3(
                        (float)(Math.Sin(theta) * Math.Sin(phi)),
                        (float)(Math.Cos(theta)),
                        (float)(Math.Sin(theta) * Math.Cos(phi)));
                    sphereVertices[i * 12 + j].Color = Color.Red;
                }
            }
            sphereVertices[60].Position = Vector3.UnitY;
            sphereVertices[60].Color = Color.Red;
            sphereVertices[61].Position = -Vector3.UnitY;
            sphereVertices[61].Color = Color.Red;

            sphereIndices = new short[13 * (6 + 5)];

            // 垂直に6枚の円
            for (var i = 0; i < 6; ++i)
            {
                sphereIndices[i * 13 + 0] = 60;
                sphereIndices[i * 13 + 6] = 61;
                sphereIndices[i * 13 + 12] = 60;
                for (var j = 0; j < 5; ++j)
                {
                    sphereIndices[i * 13 + j + 1] = (short)(j * 12 + i);
                    sphereIndices[i * 13 + 11 - j] = (short)(j * 12 + i + 6);
                }
            }

            // 水平に5枚の円
            var offset = 13 * 6;
            for (var i = 0; i < 5; ++i)
            {
                for (var j = 0; j < 12; ++j)
                {
                    sphereIndices[i * 13 + j + offset] = (short)(i * 12 + j);
                }
                sphereIndices[i * 13 + 12 + offset] = sphereIndices[i * 13 + offset];
            }

            // 円柱の頂点初期化
            var division = 12;
            cylinderVertices = new VertexPositionColor[division * 2];

            var piover6 = Math.PI / 6;
            for (var i = 0; i < division * 2; ++i)
            {
                cylinderVertices[i].Position = new Vector3(
                    (float)Math.Cos(piover6 * (i % division)),
                    (i < division) ? 0.0f : 1.0f,
                    (float)Math.Sin(piover6 * (i % division)));
                cylinderVertices[i].Color = Color.Purple;
            }

            // 縦ライン用のインデックス
            cylinderIndices = new short[division * 2];
            for (var i = 0; i < division; ++i)
            {
                cylinderIndices[i * 2 + 0] = (short)i;
                cylinderIndices[i * 2 + 1] = (short)(i + division);
            }
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            DebugSampleManager.GetInstance().BeginTimeRuler("CollisionDetection");

            // プレイヤーの攻撃と敵の食らいの判定
            Detect(CollisionGroup.PlayerAttack, CollisionGroup.EnemyDamage);

            // プレイヤーの存在と敵の存在の判定
            Detect(CollisionGroup.PlayerBody, CollisionGroup.EnemyBody);

            DebugSampleManager.GetInstance().EndTimeRuler("CollisionDetection");
        }

        public override void Draw(GameTime gameTime)
        {
            // デバッグ表示
            DebugSampleManager.GetInstance().BeginTimeRuler("DrawCollision");
            DebugDraw();
            DebugSampleManager.GetInstance().EndTimeRuler("DrawCollision");
        }

        public void Add(CollisionInfo info)
        {
            // 未登録ならID発行
            if (info.ID == 0)
            {
                if (availableIDList.Count == 0)
                {
                    // 新ID発行
                    info.ID = ++IDCount;
                }
                else
                {
                    // IDストックから発行
                    info.ID = availableIDList[availableIDList.Count - 1];
                    availableIDList.RemoveAt(availableIDList.Count - 1);
                }
                infoList.Add(info);
                if (!collisionMatrix.ContainsKey(info.ID))
                {
                    collisionMatrix[info.ID] = new Dictionary<int, int>();
                }
            }
        }

        public void Remove(CollisionInfo info)
        {
            if (infoList.Remove(info))
            {
                // ヒット回数をリセット
                Reset(info.ID);
                // IDをストックに返す
                availableIDList.Add(info.ID);
                info.ID = 0;
            }
        }

        public void Reset(int ID)
        {
            if (ID > 0)
            {
                // TODO : コスト高そう
                collisionMatrix[ID].ToList().ForEach(pair => collisionMatrix[ID][pair.Key] = 0);
            }
        }

        void Detect(CollisionGroup alpha, CollisionGroup beta)
        {
            // 衝突判定が有効なものだけ取り出すクエリ
            var activeQuery =
                from info in infoList
                where info.Active()
                select info;

            var alphaQuery =
                from a in activeQuery
                where a.Group() == alpha
                select a;

            var betaQuery =
                from b in activeQuery
                where b.Group() == beta
                select b;

            foreach (var a in alphaQuery)
            {
                foreach (var b in betaQuery)
                {
                    if (Judge(a, b))
                    {
                        // コリジョン回数マトリックスを更新
                        if (!collisionMatrix[a.ID].ContainsKey(b.ID))
                        {
                            collisionMatrix[a.ID][b.ID] = 0;
                        }
                        if (!collisionMatrix[b.ID].ContainsKey(a.ID))
                        {
                            collisionMatrix[b.ID][a.ID] = 0;
                        }

                        var ab = ++collisionMatrix[a.ID][b.ID];
                        var ba = ++collisionMatrix[b.ID][a.ID];

                        if (ab < ba)
                        {
                            collisionMatrix[b.ID][a.ID] = ba = ab;
                        }
                        if (ba < ab)
                        {
                            collisionMatrix[a.ID][b.ID] = ab = ba;
                        }

                        a.Reaction(a.ReactionArg(), b.ToOpponent, ab);
                        b.Reaction(b.ReactionArg(), a.ToOpponent, ba);
                    }
                }
            }
        }

        bool Judge(CollisionInfo a, CollisionInfo b)
        {
            if (a.Shape == CollisionShape.Void || b.Shape == CollisionShape.Void)
            {
                return false;
            }

            switch (a.Shape)
            {
                case CollisionShape.Sphere:
                    switch (b.Shape)
                    {
                        case CollisionShape.Sphere:
                            return JudgeSphereSphere(
                                a.ShapeParameter as SphereShapeParameter,
                                b.ShapeParameter as SphereShapeParameter);
                    }
                    break;
                case CollisionShape.Cylinder:
                    switch (b.Shape)
                    {
                        case CollisionShape.Cylinder:
                            return JudgeCylinderCylinder(
                                a.ShapeParameter as CylinderShapeParameter,
                                b.ShapeParameter as CylinderShapeParameter);
                    }
                    break;
            }

            return false;
        }

        // 球対球
        bool JudgeSphereSphere(SphereShapeParameter a, SphereShapeParameter b)
        {
            return ((a.Center() - b.Center()).Length() < a.Radius() + b.Radius());
        }

        // 円柱対円柱
        bool JudgeCylinderCylinder(CylinderShapeParameter a, CylinderShapeParameter b)
        {
            var baseA = a.Base();
            var baseB = b.Base();
            var horizonalA = new Vector2(baseA.X, baseA.Z);
            var horizonalB = new Vector2(baseB.X, baseB.Z);
            
            // 水平交差判定
            var horizonal = ((horizonalA - horizonalB).Length() < a.Radius() + b.Radius());
            if (!horizonal)
                return false;

            // 垂直交差判定
            var al = baseA.Y;
            var ah = al + a.Height();
            var bl = baseB.Y;
            var bh = bl + b.Height();
            var vertical =
                (al > bl && al < bh)
                || (ah > bl && ah < bh)
                || (bl > al && bl < ah)
                || (bh > al && bh < ah);

            return vertical;
        }

        [Conditional("DEBUG")]
        void DebugDraw()
        {
            var camera = CameraManager.GetInstance().Get(CameraLabel.Game);

            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Proj;

            var activeQuery =
                from x in infoList
                where x.Active()
                select x;

            DebugDrawSphere(activeQuery);
            DebugDrawCylinder(activeQuery);
        }

        [Conditional("DEBUG")]
        void DebugDrawSphere(IEnumerable<CollisionInfo> activeQuery)
        {
            var query =
                from x in activeQuery
                where x.Shape == CollisionShape.Sphere
                select x.ShapeParameter as SphereShapeParameter;

            foreach (var sphere in query)
            {
                var scaleMat = Matrix.CreateScale(sphere.Radius());
                var transMat = Matrix.CreateTranslation(sphere.Center());
                basicEffect.World = scaleMat * transMat;

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    for (var i = 0; i < 11; ++i)
                    {
                        Game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, sphereVertices, 0, sphereVertices.Length, sphereIndices, i * 13, 12);
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        void DebugDrawCylinder(IEnumerable<CollisionInfo> activeQuery)
        {
            var query =
                from x in activeQuery
                where x.Shape == CollisionShape.Cylinder
                select x.ShapeParameter as CylinderShapeParameter;

            var division = cylinderVertices.Length / 2;

            foreach (var cylinder in query)
            {
                var scaleMat = Matrix.CreateScale(
                    cylinder.Radius(),
                    cylinder.Height(),
                    cylinder.Radius());
                var transMat = Matrix.CreateTranslation(cylinder.Base());
                basicEffect.World = scaleMat * transMat;

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, cylinderVertices, 0, division - 1);
                    Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, cylinderVertices, division, division - 1);

                    Game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, cylinderVertices, 0, cylinderVertices.Length, cylinderIndices, 0, division);
                }
            }
        }
    }
}
