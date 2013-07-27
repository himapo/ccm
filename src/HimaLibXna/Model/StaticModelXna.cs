using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.Math;

namespace HimaLib.Model
{
    public class StaticModelXna : IModel
    {
        public string Name { get; set; }

        public List<string> MotionNames { get; private set; }

        public string CurrentMotionName { get; private set; }

        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        public StaticModelXna()
        {
            MotionNames = new List<string>();
        }

        public bool Init()
        {
            return true;
        }

        public void Update(float elapsedTimeSeconds)
        {
        }

        public void Render(IModelRenderParameter param)
        {
            var renderer = ModelRendererFactoryXna.Instance.Create(param);
            renderer.Render(Model);
        }

        public void AddAttachment(string name)
        {
        }

        public void RemoveAttachment(string name)
        {
        }

        public void ChangeMotion(string name, float shiftTime)
        {
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
