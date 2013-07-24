using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Render;
using HimaLib.Math;
using SkinnedModel;
using TexSkinningSample;

namespace HimaLib.Model
{
    public class DynamicModelXna : Graphics.GraphicsDeviceUser, IModel
    {
        public string Name { get; set; }

        public List<string> MotionNames { get; private set; }

        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        bool Initialized = false;

        SkinningData SkinningData;

        AnimationPlayer AnimationPlayer;

        FlipTexture2D RotationTexture;

        FlipTexture2D TranslationTexture;

        DefaultModelRendererXna Renderer = new DefaultModelRendererXna();

        public DynamicModelXna()
        {
            MotionNames = new List<string>();
        }

        public void Update(float elapsedTimeSeconds)
        {
            if (!Init())
            {
                return;
            }

            AnimationPlayer.Update(TimeSpan.FromSeconds(elapsedTimeSeconds), true);
        }

        bool Init()
        {
            if (Initialized)
            {
                return true;
            }

            if (Model == null)
            {
                return false;
            }

            SkinningData = Model.Tag as SkinningData;

            if (SkinningData == null)
                throw new InvalidOperationException
                    ("このモデルのTagにはSkinningDataが設定されていません。\n" +
                        "プロセッサにSkinnedModelProcessorを指定してください");

            if (SkinningData.AnimationClips.Count == 0)
                throw new InvalidOperationException
                    ("このモデルにはAnimationClipが存在しません。");

            AnimationPlayer = new AnimationPlayer(SkinningData);

            MotionNames.AddRange(SkinningData.AnimationClips.Keys);

            // 適当に最初のモーションを再生
            foreach (var clip in SkinningData.AnimationClips.Values)
            {
                AnimationPlayer.StartClip(clip);
                break;
            }

            int width = AnimationPlayer.GetSkinRotations().Length;
            int height = 1;

            RotationTexture = new FlipTexture2D(GraphicsDevice, width, height, false, SurfaceFormat.Vector4);

            TranslationTexture = new FlipTexture2D(GraphicsDevice, width, height, false, SurfaceFormat.Vector4);

            Initialized = true;

            return true;
        }

        public void Render(IModelRenderParameter param)
        {
            if (!Initialized)
            {
                return;
            }

            RotationTexture.Flip();
            TranslationTexture.Flip();

            RotationTexture.Texture.SetData<Microsoft.Xna.Framework.Quaternion>(AnimationPlayer.GetSkinRotations());
            TranslationTexture.Texture.SetData<Microsoft.Xna.Framework.Vector4>(AnimationPlayer.GetSkinTraslations());

            var textureSize = new Vector2(RotationTexture.Texture.Width, RotationTexture.Texture.Height);

            // TODO : 任意のレンダラでの描画

            var renderParam = param as DefaultModelRenderParameter;

            if (renderParam == null)
            {
                return;
            }

            renderParam.ParametersTexture["BoneRotationTexture"] = RotationTexture.Texture;
            renderParam.ParametersTexture["BoneTranslationTexture"] = TranslationTexture.Texture;
            renderParam.ParametersVector2["BoneTextureSize"] = textureSize;

            Renderer.SetParameter(renderParam);
            Renderer.Render(Model);
        }

        public void AddAttachment(string name)
        {
        }

        public void RemoveAttachment(string name)
        {
        }

        public void ChangeMotion(string name, float shiftTime)
        {
            AnimationClip clip = SkinningData.AnimationClips[name];
            AnimationPlayer.StartClip(clip);
        }

        public Matrix GetBoneMatrix(string name)
        {
            return Matrix.Identity;
        }

        public Matrix GetAttachmentMatrix(string name)
        {
            return Matrix.Identity;
        }
    }
}
