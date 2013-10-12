using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Render;
using HimaLib.Camera;
using HimaLib.Model;
using HimaLib.Updater;
using HimaLib.Debug;
using HimaLib.Texture;
using ccm.Game;
using ccm.Render;

namespace ccm.Player
{
    public class PlayerDrawer : IPlayerDrawer
    {
        public CameraBase Camera { get; set; }

        ToonModelRenderParameter toonRenderParam = new ToonModelRenderParameter();

        DefaultModelRenderParameter defaultRenderParam = new DefaultModelRenderParameter();

        IBillboard Billboard = BillboardFactory.Instance.Create();

        SimpleBillboardRenderParameter BillboardRenderParam = new SimpleBillboardRenderParameter();

        float Alpha = 0.5f;

        public PlayerDrawer()
        {
            CreateUpdater();
            InitDefaultRenderer();
        }

        void CreateUpdater()
        {
            new HimaLib.Updater.SinUpdater(
                HimaLib.Updater.UpdaterManager.Instance,
                1000.0f,
                0.0f,
                360.0f,
                0.5f,
                0.5f,
                (a) => { Alpha = a; },
                () => { CreateUpdater(); });
        }

        void InitDefaultRenderer()
        {
            defaultRenderParam.IsShadowCaster = true;
            defaultRenderParam.IsShadowReceiver = true;
            defaultRenderParam.GBufferEnabled = true;

            //defaultRenderParam.ParametersVector3["Light1Direction"] = Vector3.One * -1.0f;
            //defaultRenderParam.ParametersVector3["Light1Color"] = new Vector3(0.8f, 0.9f, 0.7f);
            defaultRenderParam.ParametersVector3["Light2Direction"] = Vector3.One * -1.0f;
            defaultRenderParam.ParametersVector3["Light2Color"] = new Vector3(0.0f, 0.0f, 0.0f);
            defaultRenderParam.ParametersVector3["AmbientColor"] = new Vector3(0.1f, 0.1f, 0.1f);
        }

        public void Draw(Player player)
        {
            Draw(player.Model, player.Transform);
            DrawCombo(player.ComboCounter.Count, player.Transform);
        }

        void Draw(IModel model, AffineTransform transform)
        {
            toonRenderParam.Camera = Camera;
            toonRenderParam.Transform = transform;

            defaultRenderParam.Transform = new AffineTransform()
            {
                Scale = transform.Scale,
                Rotation = new Vector3(transform.Rotation.X + MathUtil.ToRadians(-90.0f), transform.Rotation.Y, transform.Rotation.Z),
                Translation = transform.Translation,
            };
            defaultRenderParam.ParametersMatrix["World"] = defaultRenderParam.Transform.WorldMatrix;

            ModelRenderParameter renderParam = defaultRenderParam;

            RenderSceneManager.Instance.RenderModel(model, renderParam);

            BillboardRenderParam.Camera = Camera;
            BillboardRenderParam.Alpha = Alpha;
            BillboardRenderParam.Transform = new AffineTransform();
            BillboardRenderParam.Transform.Scale = Vector3.One * 0.004f;
            BillboardRenderParam.Transform.Rotation = Vector3.Zero;
            BillboardRenderParam.Transform.Translation = new Vector3(
                transform.Translation.X + 4.0f,
                transform.Translation.Y + 8.5f,
                transform.Translation.Z + 1.0f);
            BillboardRenderParam.Texture = TextureFactory.Instance.CreateFromImage("Texture/miki");

            Billboard.Render(BillboardRenderParam);
        }

        void DrawCombo(int count, AffineTransform transform)
        {
            if (count < 2)
            {
                return;
            }

            var screenPosition = MathUtil.Project(
                transform.Translation,
                Camera.View,
                Camera.Projection,
                GameProperty.resolutionWidth,
                GameProperty.resolutionHeight);

            DebugFont.Add(string.Format("{0} Hit", count), 
                screenPosition.X - 20.0f, 
                screenPosition.Y - 120.0f);
        }
    }
}
