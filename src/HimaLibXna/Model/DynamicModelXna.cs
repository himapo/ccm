using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.System;
using SkinnedModel;
using TexSkinningSample;

namespace HimaLib.Model
{
    public class DynamicModelXna : IModel
    {
        public string Name { get; set; }

        public List<string> MotionNames { get; private set; }

        public string CurrentMotionName { get; private set; }

        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        bool Initialized = false;

        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        SkinningData SkinningData;

        AnimationPlayer AnimationPlayer;

        QuatTransform[] BoneTransforms;

        FlipTexture2D RotationTexture;

        FlipTexture2D TranslationTexture;

        Microsoft.Xna.Framework.Vector2 TextureSize = new Microsoft.Xna.Framework.Vector2();

        DefaultModelRendererXna Renderer = new DefaultModelRendererXna();

        public DynamicModelXna()
        {
            MotionNames = new List<string>();
        }

        public void Update(float elapsedTimeSeconds)
        {
            if (!Initialized)
            {
                return;
            }

            AnimationPlayer.Update(TimeSpan.FromSeconds(elapsedTimeSeconds), true);

            AnimationPlayer.GetBoneTransforms().CopyTo(BoneTransforms, 0);

            RotationTexture.Flip();
            TranslationTexture.Flip();

            RotationTexture.Texture.SetData<Microsoft.Xna.Framework.Quaternion>(AnimationPlayer.GetSkinRotations());
            TranslationTexture.Texture.SetData<Microsoft.Xna.Framework.Vector4>(AnimationPlayer.GetSkinTraslations());

            TextureSize.X = RotationTexture.Texture.Width;
            TextureSize.Y = RotationTexture.Texture.Height;
        }

        public bool Init()
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
            foreach (var clip in SkinningData.AnimationClips)
            {
                AnimationPlayer.StartClip(clip.Value);
                CurrentMotionName = clip.Key;
                break;
            }

            BoneTransforms = new QuatTransform[SkinningData.BindPose.Count];

            int width = AnimationPlayer.GetSkinRotations().Length;
            int height = 1;

            RotationTexture = new FlipTexture2D(GraphicsDevice, width, height, false, SurfaceFormat.Vector4);

            TranslationTexture = new FlipTexture2D(GraphicsDevice, width, height, false, SurfaceFormat.Vector4);

            Initialized = true;

            return true;
        }

        public void Render(ModelRenderParameter param)
        {
            if (!Initialized)
            {
                return;
            }

            var renderer = ModelRendererFactoryXna.Instance.Create(param);

            var skinnedModelRenderer = renderer as SkinnedModelRendererXna;

            if (skinnedModelRenderer == null)
            {
                return;
            }

            skinnedModelRenderer.BoneRotationTexture = RotationTexture.Texture;
            skinnedModelRenderer.BoneTranslationTexture = TranslationTexture.Texture;
            skinnedModelRenderer.BoneTextureSize = TextureSize;

            renderer.RenderDynamic(Model);
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
            CurrentMotionName = name;
        }

        public Matrix GetBoneMatrix(string name)
        {
            var boneIndex = SkinningData.BoneIndices[name];
            var transform = BoneTransforms[boneIndex];
            return MathUtilXna.ToHimaLibMatrix(transform.ToMatrix());
        }

        public Matrix GetAttachmentMatrix(string name)
        {
            return Matrix.Identity;
        }
    }
}
