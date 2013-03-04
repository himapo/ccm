using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Updater
{
    public interface IUpdaterManager
    {
        void Add(IUpdater updater);
    }
}
