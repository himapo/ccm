using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Model;

namespace ccm.Ally
{
    public interface IAllyDrawer
    {
        void Draw(IModel model, AffineTransform transform);
    }
}
