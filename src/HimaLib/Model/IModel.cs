using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.System;
using HimaLib.Math;

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

        /// <summary>
        /// モデル空間でのボーンの姿勢行列を返す
        /// </summary>
        /// <param name="name">ボーン名</param>
        /// <returns></returns>
        Matrix GetBoneMatrix(string name);

        /// <summary>
        /// ワールド空間での装備の姿勢行列を返す
        /// </summary>
        /// <param name="name">装備名</param>
        /// <returns></returns>
        Matrix GetAttachmentMatrix(string name);
    }
}
