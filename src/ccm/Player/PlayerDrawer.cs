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
using ccm.Game;

namespace ccm.Player
{
    public class PlayerDrawer : IPlayerDrawer
    {
        public ICamera Camera { get; set; }

        ToonModelRenderParameter renderParam = new ToonModelRenderParameter();

        IBillboard Billboard = BillboardFactory.Instance.Create();

        SimpleBillboardRenderParameter BillboardRenderParam = new SimpleBillboardRenderParameter();

        float Alpha = 0.5f;

        public PlayerDrawer()
        {
            CreateUpdater();
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

        void Draw(IModel model, AffineTransform transform)
        {
            renderParam.Camera = Camera;
            renderParam.Transform = transform;

            model.Render(renderParam);

            BillboardRenderParam.Camera = Camera;
            BillboardRenderParam.Alpha = Alpha;
            BillboardRenderParam.Transform = new AffineTransform();
            BillboardRenderParam.Transform.Scale = Vector3.One * 0.004f;
            BillboardRenderParam.Transform.Rotation = Vector3.Zero;
            BillboardRenderParam.Transform.Translation = new Vector3(
                transform.Translation.X + 4.0f,
                transform.Translation.Y + 8.5f,
                transform.Translation.Z + 1.0f);
            BillboardRenderParam.TextureName = "Texture/miki";

            Billboard.Render(BillboardRenderParam);
        }

        public void Draw(Player player)
        {
            Draw(player.Model, player.Transform);
            DrawCombo(player.ComboCounter.Count, player.Transform);
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
