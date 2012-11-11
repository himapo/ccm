using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BulletX.BulletCollision.BroadphaseCollision;
using BulletX.BulletCollision.CollisionDispatch;
using BulletX.BulletCollision.CollisionShapes;
using BulletX.BulletDynamics.ConstraintSolver;
using BulletX.BulletDynamics.Dynamics;
using BulletX.LinerMath;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class PhysicsManager : MyGameComponent, IPhysicsService
    {
        ICollisionConfiguration collisionConfiguration;
        CollisionDispatcher dispatcher;
        IBroadphaseInterface broadphase;
        IConstraintSolver solver;
        DynamicsWorld dynamicsWorld;

        List<BulletX.BulletCollision.CollisionShapes.CollisionShape> collisionShapes;

        public PhysicsManager(Game game)
            : base(game)
        {
            collisionConfiguration = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConfiguration);
            broadphase = new DbvtBroadphase(null);
            solver = new SequentialImpulseConstraintSolver();
            dynamicsWorld = new DiscreteDynamicsWorld(dispatcher, broadphase, solver, collisionConfiguration);

            collisionShapes = new List<BulletX.BulletCollision.CollisionShapes.CollisionShape>();

            // TODO: ここで子コンポーネントを作成します。
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

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: ここにアップデートのコードを追加します。
            dynamicsWorld.stepSimulation((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        public void AddBox(Vector3 size, Vector3 position, float mass)
        {
            var shape = new BoxShape(new btVector3(size.X, size.Y, size.Z));
            collisionShapes.Add(shape);
            var transform = new btTransform();
            transform.setIdentity();
            transform.Origin = new btVector3(position.X, position.Y, position.Z);
            btVector3 localInertia;
            if (mass != 0.0f)
                shape.calculateLocalInertia(mass, out localInertia);
            else
                localInertia = new btVector3(0.0f, 0.0f, 0.0f);
            var motionState = new DefaultMotionState(transform);
            var rbInfo = new RigidBodyConstructionInfo(mass, motionState, shape, localInertia);
            var body = new RigidBody(rbInfo);
            dynamicsWorld.addRigidBody(body);
        }
    }
}
