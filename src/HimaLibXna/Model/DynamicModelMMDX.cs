using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.Core.Motion;
using MikuMikuDance.XNA.Model;
using MikuMikuDance.XNA.Accessory;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Content;

namespace HimaLib.Model
{
    public class DynamicModelMMDX : IModel
    {
        public string Name { get; set; }

        public MMDXModel Model { get; set; }

        Dictionary<string, MMDAccessory> accessoryModels = new Dictionary<string, MMDAccessory>();

        MMDAccessoryLoader accessoryLoader = new MMDAccessoryLoader();

        MMDVACLoader vacLoader = new MMDVACLoader();

        MMDMotionLoader motionLoader = new MMDMotionLoader();

        string nowMotion = "";
        string prevMotion = "";
        float shiftTime = 0.0f;
        float elapsedTime = 0.0f;

        HashSet<string> motionNames = new HashSet<string>();

        public DynamicModelMMDX()
        {
        }

        public void Update(float elapsedTimeSeconds)
        {
            elapsedTime += elapsedTimeSeconds;
            elapsedTime = MathUtil.Clamp(elapsedTime, 0.0f, shiftTime);

            Model.AnimationPlayer[nowMotion].BlendingFactor = elapsedTime / shiftTime;
            if (prevMotion != "")
            {
                Model.AnimationPlayer[prevMotion].BlendingFactor = 1.0f - elapsedTime / shiftTime;
            }
        }

        public void Render(IModelRenderParameter param)
        {
            var renderer = ModelRendererFactoryMMDX.Instance.Create(param);
            renderer.Render(Model);

            foreach (var accessory in accessoryModels.Values)
            {
                renderer.Render(accessory);
            }
        }

        public void AddAttachment(string name)
        {
            var accessory = accessoryLoader.Load("Accessory/" + name);
            var vac = vacLoader.Load("Accessory/" + name + "-vac");
            Model.BindAccessory(accessory, vac);
            accessoryModels[name] = accessory;
        }

        public void RemoveAttachment(string name)
        {
            
        }

        public void ChangeMotion(string name, float shiftTime)
        {
            if (name == nowMotion)
                return;

            if (prevMotion != "")
                Model.AnimationPlayer[prevMotion].BlendingFactor = 0.0f;

            LoadMotion(name);

            prevMotion = nowMotion;
            nowMotion = name;
            this.shiftTime = shiftTime;
            elapsedTime = 0.0f;

            //再生した後ならリセットをかける
            if (Model.AnimationPlayer[nowMotion].NowFrame > 0)
            {
                //停止
                Model.AnimationPlayer[nowMotion].Stop();
                //巻き戻し
                Model.AnimationPlayer[nowMotion].Reset();
                //剛体位置のリセット
                //Model.PhysicsManager.Reset();
            }
            //モーションの再生
            Model.AnimationPlayer[nowMotion].Start(true);
        }

        void LoadMotion(string name)
        {
            if (motionNames.Contains(name))
            {
                return;
            }

            if (!Model.AnimationPlayer.ContainsKey(name))
            {
                var motion = motionLoader.Load("Motion/" + name);
                Model.AnimationPlayer.AddMotion(name, motion, MMDMotionTrackOptions.UpdateWhenStopped);
            }
            motionNames.Add(name);
        }

        public Matrix GetBoneMatrix(string name)
        {
            return MathUtilXna.ToHimaLibMatrix(Model.BoneManager[name].GlobalTransform);
        }

        public Matrix GetAttachmentMatrix(string name)
        {
            Microsoft.Xna.Framework.Matrix m;
            accessoryModels[name].GetPosition(out m);
            return MathUtilXna.ToHimaLibMatrix(m);
        }
    }
}
