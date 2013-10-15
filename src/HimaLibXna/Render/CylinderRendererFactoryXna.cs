using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public class CylinderRendererFactoryXna
    {
        static readonly CylinderRendererFactoryXna instance = new CylinderRendererFactoryXna();

        public static CylinderRendererFactoryXna Instance { get { return instance; } private set { } }

        Dictionary<CylinderRendererType, ICylinderRendererXna> RendererDic = new Dictionary<CylinderRendererType, ICylinderRendererXna>();

        CylinderRendererFactoryXna()
        {
        }

        public ICylinderRendererXna Create(CylinderRenderParameter param)
        {
            switch (param.Type)
            {
                case CylinderRendererType.Wire:
                    return Create<WireCylinderRenderer>(param);
                default:
                    break;
            }

            return new NullCylinderRendererXna();
        }

        ICylinderRendererXna Create<RendererType>(CylinderRenderParameter param)
            where RendererType : ICylinderRendererXna, new()
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
