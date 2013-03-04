using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Model
{
    public class BillboardFactory
    {
        static readonly BillboardFactory instance = new BillboardFactory();

        public static BillboardFactory Instance { get { return instance; } private set { } }

        BillboardFactory()
        {
        }

        public IBillboard Create()
        {
            return new BillboardXna();
        }
    }
}
