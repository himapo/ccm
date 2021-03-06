﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public class AffineTransform
    {
        public Vector3 Scale;

        public Vector3 Rotation;

        public Vector3 Translation;

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

        public AffineTransform()
            : this(Vector3.One, Vector3.Zero, Vector3.Zero)
        {
        }

        public AffineTransform(AffineTransform src)
            : this(src.Scale, src.Rotation, src.Translation)
        {
        }
    }
}
