using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    class DirectionalLight : LightBase
    {
        public Vector3 Direction { get; set; }

        public Vector3 DiffuseColor { get; set; }

        public Vector3 SpecularColor { get; set; }

        public DirectionalLight()
        {
            Direction = Vector3.Down;
            DiffuseColor = Vector3.Zero;
            SpecularColor = Vector3.Zero;
        }

        public void Update()
        {
        }
    }
}
