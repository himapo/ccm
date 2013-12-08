using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;

namespace HimaLib.Model
{
    public class FontFactory
    {
        public static FontFactory Instance
        {
            get { return Singleton<FontFactory>.Instance; }
        }

        FontFactory()
        {
        }

        public Font Create()
        {
            return new FontXna();
        }
    }
}
