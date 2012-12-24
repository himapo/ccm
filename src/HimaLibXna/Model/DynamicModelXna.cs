using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;

namespace HimaLib.Model
{
    public class DynamicModelXna : IModel
    {
        public string Name { get; set; }

        public Microsoft.Xna.Framework.Graphics.Model Model { get; set; }

        public DynamicModelXna()
        {
        }

        public void Update(float elapsedTimeSeconds)
        {
        }

        public void Render(IModelRenderParameter renderer)
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
    }
}
