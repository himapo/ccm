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

        public IBillboardRendererXna Create(BillboardRenderParameter param)
        {
            // 悔しいがダウンキャストを使う（もっといい手が見つかるまで・・・）

            switch (param.Type)
            {
                case BillboardRendererType.Simple:
                    return Create<SimpleBillboardRendererXna>(param);
                case BillboardRendererType.Hud:
                    return Create<HudBillboardRenderer>(param);
                case BillboardRendererType.Depth:
                    return Create<DepthRendererXna>(param);
                default:
                    break;
            }

            return new NullBillboardRendererXna();
        }

        IBillboardRendererXna Create<RendererType>(BillboardRenderParameter param)
            where RendererType : IBillboardRendererXna, new()
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
