using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    public interface IUpdater
    {
        void Update(ITimeKeeper timeKeeper);
    }
}
