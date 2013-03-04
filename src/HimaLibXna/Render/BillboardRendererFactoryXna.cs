using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public class BillboardRendererFactoryXna
    {
        static readonly BillboardRendererFactoryXna instance = new BillboardRendererFactoryXna();

        public static BillboardRendererFactoryXna Instance { get { return instance; } private set { } }

        Dictionary<BillboardRendererType, IBillboardRendererXna> RendererDic = new Dictionary<BillboardRendererType,IBillboardRendererXna>();

        BillboardRendererFactoryXna()
        {
        }

        public IBillboardRendererXna Create(IBillboardRenderParameter param)
        {
            // 悔しいがダウンキャストを使う（もっといい手が見つかるまで・・・）

            switch (param.Type)
            {
                case BillboardRendererType.Simple:
                    {
                        if (!RendererDic.ContainsKey(param.Type))
                        {
                            RendererDic[param.Type] = new SimpleBillboardRendererXna();
                        }
                        var result = RendererDic[param.Type] as SimpleBillboardRendererXna;
                        result.SetParameter(param as SimpleBillboardRenderParameter);
                        return result;
                    }
                default:
                    break;
            }

            return new NullBillboardRendererXna();
        }
    }
}
