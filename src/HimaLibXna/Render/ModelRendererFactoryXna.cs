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

        ModelRendererFactoryXna()
        {
        }

        public IModelRendererXna Create(IModelRenderParameter param)
        {
            // 悔しいがダウンキャストを使う（もっといい手が見つかるまで・・・）

            switch (param.Type)
            {
                case ModelRendererType.Simple:
                    var result = new SimpleModelRendererXna();
                    result.SetParameter(param as SimpleModelRenderParameter);
                    return result;
                default:
                    break;
            }

            return new NullModelRendererXna();
        }
    }
}
