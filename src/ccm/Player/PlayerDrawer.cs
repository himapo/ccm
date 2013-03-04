using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Render;
using HimaLib.Camera;
using HimaLib.Model;

namespace ccm.Player
{
    public class PlayerDrawer : IPlayerDrawer
    {
        public ICamera Camera { get; set; }

        ToonModelRenderParameter renderParam = new ToonModelRenderParameter();

        IBillboard Billboard = BillboardFactory.Instance.Create();

        SimpleBillboardRenderParameter BillboardRenderParam = new SimpleBillboardRenderParameter();

        public PlayerDrawer()
        {
        }

        public void Draw(IModel model, AffineTransform transform)
        {
            renderParam.Camera = Camera;
            renderParam.Transform = transform;

            model.Render(renderParam);

            BillboardRenderParam.Camera = Camera;
            BillboardRenderParam.Alpha = 1.0f;
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

    }
}
