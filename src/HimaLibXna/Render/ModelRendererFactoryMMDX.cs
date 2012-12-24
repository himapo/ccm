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

        ModelRendererFactoryMMDX()
        {
        }

        public IModelRendererMMDX Create(IModelRenderParameter param)
        {
            // 悔しいがダウンキャストを使う（もっといい手が見つかるまで・・・）

            switch (param.Type)
            {
                case ModelRendererType.Toon:
                    var result = new ToonModelRendererMMDX();
                    result.SetParameter(param as ToonModelRenderParameter);
                    return result;
                default:
                    break;
            }

            return new NullModelRendererMMDX();
        }
    }
}
