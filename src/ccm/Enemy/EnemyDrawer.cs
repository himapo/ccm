using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;
using HimaLib.Render;
using HimaLib.Camera;

namespace ccm.Enemy
{
    public class EnemyDrawer : IEnemyDrawer
    {
        public ICamera Camera { get; set; }

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
            RenderParam.DirLight0Direction = new Vector3(0.4f, -0.5f, -0.3f);
            RenderParam.DirLight0Direction.Normalize();
            RenderParam.DirLight0DiffuseColor = Vector3.One;
        }

        public void Draw(Enemy enemy)
        {
            RenderParam.Camera = Camera;
            RenderParam.Transform = enemy.Transform;
            //RenderParam.Alpha = 0.5f;
            enemy.Model.Render(RenderParam);
        }
    }
}
