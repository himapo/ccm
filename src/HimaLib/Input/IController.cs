using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Input
{
    public interface IController
    {
        bool IsPush(int label);

        bool IsPress(int label);

        bool IsRelease(int label);

        int GetX(int label);

        int GetY(int label);

        int GetMoveX(int label);

        int GetMoveY(int label);
    }
}
