using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class DefaultModelRenderParameter : IModelRenderParameter
    {
        public ModelRendererType Type { get { return ModelRendererType.Default; } }

        public Dictionary<string, bool> ParametersBoolean { get; private set; }

        public Dictionary<string, int> ParametersInt32 { get; private set; }

        public Dictionary<string, float> ParametersSingle { get; private set; }

        public Dictionary<string, Vector2> ParametersVector2 { get; private set; }

        public Dictionary<string, Vector3> ParametersVector3 { get; private set; }

        public Dictionary<string, Vector4> ParametersVector4 { get; private set; }

        public Dictionary<string, Matrix> ParametersMatrix { get; private set; }

        public Dictionary<string, string> ParametersTexture { get; private set; }

        // TODO : Texture

        public DefaultModelRenderParameter()
        {
            ParametersBoolean = new Dictionary<string, bool>();
            ParametersInt32 = new Dictionary<string, int>();
            ParametersSingle = new Dictionary<string, float>();
            ParametersVector2 = new Dictionary<string, Vector2>();
            ParametersVector3 = new Dictionary<string, Vector3>();
            ParametersVector4 = new Dictionary<string, Vector4>();
            ParametersMatrix = new Dictionary<string, Matrix>();
            ParametersTexture = new Dictionary<string, string>();
        }
    }
}
