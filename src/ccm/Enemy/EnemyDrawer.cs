using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Render;
using HimaLib.Camera;
using HimaLib.Texture;
using ccm.Render;

namespace ccm.Enemy
{
    public class EnemyDrawer : IEnemyDrawer
    {
        public CameraBase Camera { get; set; }

        public float Alpha
        {
            get { return RenderParam.Alpha; }
            set { RenderParam.Alpha = value; }
        }

        SimpleModelRenderParameter RenderParam = new SimpleModelRenderParameter();

        public EnemyDrawer()
        {
            InitRenderParam();
        }

        void InitRenderParam()
        {
            RenderParam.Alpha = 1.0f;
            RenderParam.AmbientLightColor = Vector3.One * 0.2f;
            RenderParam.ShadowMap = TextureFactory.Instance.CreateRenderTarget((int)RenderTargetType.ShadowMap0);
        }

        public void Draw(Enemy enemy)
        {
            RenderParam.Camera = Camera;
            RenderParam.Transform = enemy.Transform.WorldMatrix;
            //RenderParam.Alpha = 0.5f;
            RenderManagerAccessor.Instance.RenderModel(enemy.Model, RenderParam);
        }
    }
}
