using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    interface IPhysicsService
    {
        void AddBox(Vector3 size, Vector3 position, float mass);
    }
}
