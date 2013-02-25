using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public IModelRendererXna Create(IModelRenderParameter param)
        {
            // 悔しいがダウンキャストを使う（もっといい手が見つかるまで・・・）

            switch (param.Type)
            {
                case ModelRendererType.Simple:
                    {
                        if (!RendererDic.ContainsKey(param.Type))
                        {
                            RendererDic[param.Type] = new SimpleModelRendererXna();
                        }
                        var result = RendererDic[param.Type] as SimpleModelRendererXna;
                        result.SetParameter(param as SimpleModelRenderParameter);
                        return result;
                    }
                case ModelRendererType.SimpleInstancing:
                    {
                        if (!RendererDic.ContainsKey(param.Type))
                        {
                            RendererDic[param.Type] = new SimpleInstancingRendererXna();
                        }
                        var result = RendererDic[param.Type] as SimpleInstancingRendererXna;
                        result.SetParameter(param as SimpleInstancingRenderParameter);
                        return result;
                    }
                default:
                    break;
            }

            return new NullModelRendererXna();
        }
    }
}
