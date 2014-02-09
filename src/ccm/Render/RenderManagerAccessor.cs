using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimaLib.Render;
using HimaLib.System;
using HimaLib.Model;
using HimaLib.Light;

namespace ccm.Render
{
    public class RenderManagerAccessor
    {
        static RenderManager instance;

        public static RenderManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = RenderManagerFactory.Instance.Create();
                }

                return instance;
            }
        }

        RenderManagerAccessor()
        {
        }
    }
}
