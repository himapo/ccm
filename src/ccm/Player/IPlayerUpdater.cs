using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Player
{
    public interface IPlayerUpdater
    {
        void Update(IModel model, AffineTransform transform);
    }
}
