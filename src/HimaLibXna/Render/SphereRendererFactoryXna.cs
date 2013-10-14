using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public class SphereRendererFactoryXna
    {
        static readonly SphereRendererFactoryXna instance = new SphereRendererFactoryXna();

        public static SphereRendererFactoryXna Instance { get { return instance; } private set { } }

        Dictionary<SphereRendererType, ISphereRendererXna> RendererDic = new Dictionary<SphereRendererType, ISphereRendererXna>();

        SphereRendererFactoryXna()
        {
        }

        public ISphereRendererXna Create(SphereRenderParameter param)
        {
            switch (param.Type)
            {
                case SphereRendererType.Wire:
                    return Create<WireSphereRenderer>(param);
                default:
                    break;
            }

            return new NullSphereRendererXna();
        }

        ISphereRendererXna Create<RendererType>(SphereRenderParameter param)
            where RendererType : ISphereRendererXna, new()
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
