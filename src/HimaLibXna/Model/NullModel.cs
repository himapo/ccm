using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.Math;

namespace HimaLib.Model
{
    public class NullModel : IModel
    {
        public string Name { get; set; }

        public List<string> MotionNames { get; private set; }

        public string CurrentMotionName { get; private set; }

        public NullModel()
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

        public void Render(ModelRenderParameter renderer)
        {
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
