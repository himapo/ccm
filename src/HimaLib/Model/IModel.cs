using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.System;

namespace HimaLib.Model
{
    public interface IModel
    {
        string Name { get; set; }

        void Update(float elapsedTimeSeconds);

        void Render(IModelRenderParameter renderer);

        void AddAttachment(string name);

        void RemoveAttachment(string name);

        void ChangeMotion(string name, float shiftTime);
    }
}
