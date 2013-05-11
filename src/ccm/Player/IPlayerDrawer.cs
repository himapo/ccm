using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Player
{
    public interface IPlayerDrawer
    {
        void Draw(Player player);
    }
}
