using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public class ModelRendererFactoryXna
    {
        static readonly ModelRendererFactoryXna instance = new ModelRendererFactoryXna();

        public static ModelRendererFactoryXna Instance { get { return instance; } private set { } }

        Dictionary<ModelRendererType, IModelRendererXna> RendererDic = new Dictionary<ModelRendererType, IModelRendererXna>();

        ModelRendererFactoryXna()
        {
        }

        public IModelRendererXna Create(ModelRenderParameter param)
        {
            switch (param.Type)
            {
                case ModelRendererType.Simple:
                    return Create<SimpleModelRendererXna>(param);
                case ModelRendererType.SimpleInstancing:
                    return Create<SimpleInstancingRendererXna>(param);
                case ModelRendererType.Default:
                    return Create<DefaultModelRendererXna>(param);
                case ModelRendererType.Depth:
                    return Create<DepthRendererXna>(param);
                case ModelRendererType.GBuffer:
                    return Create<GBufferRendererXna>(param);
                case ModelRendererType.PointLight:
                    return Create<LightBufferRenderer>(param);
                default:
                    break;
            }

            return new NullModelRendererXna();
        }

        IModelRendererXna Create<RendererType>(ModelRenderParameter param)
            where RendererType : IModelRendererXna, new()
        {
            if (!RendererDic.ContainsKey(param.Type))
            {
                RendererDic[param.Type] = new RendererType();
            }
            var result = RendererDic[param.Type];
            result.SetParameter(param);
            return result;
        }
    }
}
