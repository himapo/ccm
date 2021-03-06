﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using HimaLib.Texture;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class RenderDeviceXna : IRenderDevice
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        int PrevRenderTargetIndex;

        public void ClearAll(Color color)
        {
            GraphicsDevice.Clear(MathUtilXna.ToXnaColor(color));
        }

        public void ClearColor(Color color)
        {
            GraphicsDevice.Clear(ClearOptions.Target, MathUtilXna.ToXnaColor(color), 0.0f, 0);
        }

        public void ClearDepth()
        {
            GraphicsDevice.Clear(ClearOptions.DepthBuffer, Microsoft.Xna.Framework.Color.Gray, 1.0f, 0);
        }

        public void SetDepthState(bool depthTestEnabled, bool depthWriteEnabled)
        {
            if (depthWriteEnabled)
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else if (depthTestEnabled)
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            }
            else
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.None;
            }
        }

        public void SetRenderTarget(int index)
        {
            if (index == PrevRenderTargetIndex)
            {
                return;
            }
            PrevRenderTargetIndex = index;
            GraphicsDevice.SetRenderTarget(index == 0 ? null : GetRenderTarget(index));
        }

        public void SetRenderTargets(int[] indices)
        {
            if (indices.Length == 1)
            {
                SetRenderTarget(indices[0]);
                return;
            }

            var targets = new RenderTargetBinding[indices.Length];

            for (var i = 0; i < indices.Length; ++i)
            {
                targets[i] = new RenderTargetBinding(GetRenderTarget(indices[i]));
            }

            PrevRenderTargetIndex = -1;

            GraphicsDevice.SetRenderTargets(targets);
        }

        RenderTarget2D GetRenderTarget(int index)
        {
            return RenderTargetManager.Instance.GetRenderTarget(index);
        }
    }
}
