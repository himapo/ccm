using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public class ModelRendererFactoryMMDX
    {
        static readonly ModelRendererFactoryMMDX instance = new ModelRendererFactoryMMDX();

        public static ModelRendererFactoryMMDX Instance { get { return instance; } set { } }

        Dictionary<ModelRendererType, IModelRendererMMDX> RendererDic = new Dictionary<ModelRendererType, IModelRendererMMDX>();

        ModelRendererFactoryMMDX()
        {
        }

        public IModelRendererMMDX Create(ModelRenderParameter param)
        {
            // 悔しいがダウンキャストを使う（もっといい手が見つかるまで・・・）

            switch (param.Type)
            {
                case ModelRendererType.Toon:
                    if (!RendererDic.ContainsKey(param.Type))
                    {
                        RendererDic[param.Type] = new ToonModelRendererMMDX();
                    }
                    var result = RendererDic[param.Type] as ToonModelRendererMMDX;
                    result.SetParameter(param as ToonModelRenderParameter);
                    return result;
                default:
                    break;
            }

            return new NullModelRendererMMDX();
        }
    }
}
