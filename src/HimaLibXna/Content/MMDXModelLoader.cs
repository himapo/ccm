using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuDance.XNA.Model;

namespace HimaLib.Content
{
    public class MMDXModelLoader : ContentLoader<MMDXModel>
    {
        public static void Dispose()
        {
            foreach (var model in resourceDic.Values)
            {
                model.Dispose();
            }
        }
    }
}
