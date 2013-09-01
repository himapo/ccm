using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;

namespace HimaLib.Render
{
    public class RenderPathFactory
    {
        public static RenderPathFactory Instance
        {
            get
            {
                return Singleton<RenderPathFactory>.Instance;
            }
        }

        RenderPathFactory()
        {
        }

        public IRenderPath CreatePath(string name)
        {
            return new RenderPathXna()
            {
                Name = name,
            };
        }
    }
}
