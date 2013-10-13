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
            toonRenderParam.Transform = transform.WorldMatrix;

            defaultRenderParam.Transform = Matrix.CreateRotationX(MathUtil.ToRadians(-90.0f)) * transform.WorldMatrix;
            defaultRenderParam.ParametersMatrix["World"] = defaultRenderParam.Transform;

            ModelRenderParameter renderParam = defaultRenderParam;

            RenderSceneManager.Instance.RenderModel(model, renderParam);

            BillboardRenderParam.Camera = Camera;
            BillboardRenderParam.Alpha = Alpha;

            var playerTranslation = transform.Translation;

            BillboardRenderParam.Transform = new AffineTransform(
                Vector3.One * 0.004f,
                Vector3.Zero,
                new Vector3(
                    playerTranslation.X + 4.0f,
                    playerTranslation.Y + 8.5f,
                    playerTranslation.Z + 1.0f));
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
