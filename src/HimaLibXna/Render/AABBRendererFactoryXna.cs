using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public class AABBRendererFactoryXna
    {
        static readonly AABBRendererFactoryXna instance = new AABBRendererFactoryXna();

        public static AABBRendererFactoryXna Instance { get { return instance; } private set { } }

        Dictionary<AABBRendererType, IAABBRendererXna> RendererDic = new Dictionary<AABBRendererType, IAABBRendererXna>();

        AABBRendererFactoryXna()
        {
        }

        public IAABBRendererXna Create(AABBRenderParameter param)
        {
            switch (param.Type)
            {
                case AABBRendererType.Wire:
                    return Create<WireAABBRenderer>(param);
                default:
                    break;
            }

            return new NullAABBRendererXna();
        }

        IAABBRendererXna Create<RendererType>(AABBRenderParameter param)
            where RendererType : IAABBRendererXna, new()
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
