using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public class AffineTransform
    {
        public Vector3 Scale { get; set; }

        public Vector3 Rotation { get; set; }

        public Vector3 Translation { get; set; }

        public Matrix WorldMatrix
        {
            get
            {
                return Matrix.CreateScale(Scale) *
                    Matrix.CreateRotationZ(Rotation.Z) *
                    Matrix.CreateRotationY(Rotation.Y) *
                    Matrix.CreateRotationX(Rotation.X) *
                    Matrix.CreateTranslation(Translation);
            }
        }

        public AffineTransform(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            Scale = scale;
            Rotation = rotation;
            Translation = translation;
        }
    }
}
