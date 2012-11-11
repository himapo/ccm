using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ccm
{
    class MapCubeModel
    {
        public Model Model { get; private set; }

        public Matrix[] ModelBones { get; private set; }

        public MapCubeModel()
        {
        }

        public void Load(string modelName)
        {
            Model = ResourceManager.GetInstance().Load<Model>(modelName);

            ModelBones = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(ModelBones);
        }

    }
}
