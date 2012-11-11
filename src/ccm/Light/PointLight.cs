using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    class PointLight : LightBase
    {
        public Vector3 Center { get; set; }

        public float Decay { get; set; }

        public Vector3 DiffuseColor { get; set; }

        public Vector3 SpecularColor { get; set; }

        public PointLight()
        {
            Center = Vector3.Zero;
            Decay = 1.0f;
            DiffuseColor = Vector3.Zero;
            SpecularColor = Vector3.Zero;
        }

        public void Update()
        {
        }
    }
}
