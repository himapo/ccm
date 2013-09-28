using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Texture;

namespace HimaLib.Render
{
    public class OpaqueRenderPath : RenderPath
    {
        public ITexture ShadowMap { get; set; }

        public ITexture DiffuseLightMap { get; set; }

        public ITexture SpecularLightMap { get; set; }

        public OpaqueRenderPath()
        {
            ClearEnabled = true;
            ClearColor = Color.Gray;

            DepthSortEnabled = false;
            DepthTestEnabled = true;
            DepthWriteEnabled = true;
            DepthClearEnabled = true;

            RenderModelEnabled = true;
            RenderShadowModelOnly = false;
            RenderOpaqueModelOnly = true;
            RenderTranslucentModelOnly = false;
            RenderBillboardEnabled = true;
            RenderShadowBillboardOnly = false;
            RenderOpaqueBillboardOnly = true;
            RenderTranslucentBillboardOnly = false;
            RenderNoHudBillboardOnly = true;
            RenderHudBillboardOnly = false;
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
