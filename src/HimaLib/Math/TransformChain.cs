using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public class TransformChain
    {
        List<Vector3> Scales = new List<Vector3>();

        List<Vector3> Rotations = new List<Vector3>();

        List<Vector3> Translations = new List<Vector3>();

        public Vector3 Scale
        {
            get
            {
                var result = Vector3.One;

                Scales.ForEach((v) =>
                {
                    result.X *= v.X;
                    result.Y *= v.Y;
                    result.Z *= v.Z;
                });

                return result;
            }
        }

        public Vector3 Translation
        {
            get
            {
                var result = Vector3.Zero;

                Translations.ForEach((v) =>
                {
                    result.X += v.X;
                    result.Y += v.Y;
                    result.Z += v.Z;
                });

                return result;
            }
        }

        public Matrix WorldMatrix
        {
            get
            {
                var result = Matrix.Identity;
                
                Scales.ForEach((v) =>
                {
                    result *= Matrix.CreateScale(v);
                });

                Rotations.ForEach((v) =>
                {
                    result *= Matrix.CreateRotationZ(v.Z);
                    result *= Matrix.CreateRotationY(v.Y);
                    result *= Matrix.CreateRotationX(v.X);
                });

                Translations.ForEach((v) =>
                {
                    result *= Matrix.CreateTranslation(v);
                });

                return result;
            }
        }

        public TransformChain()
        {
        }

        public TransformChain(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            Push(scale, rotation, translation);
        }

        public TransformChain(TransformChain src)
        {
            Push(src);
        }

        public void Clear()
        {
            ClearScale();
            ClearRotation();
            ClearTranslation();
        }

        public void ClearScale()
        {
            Scales.Clear();
        }

        public void ClearRotation()
        {
            Rotations.Clear();
        }

        public void ClearTranslation()
        {
            Translations.Clear();
        }

        public void Push(TransformChain transform)
        {
            Scales.AddRange(transform.Scales);
            Rotations.AddRange(transform.Rotations);
            Translations.AddRange(transform.Translations);
        }

        public void Push(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            PushScale(scale);
            PushRotation(rotation);
            PushTranslation(translation);
        }

        public void PushScale(Vector3 scale)
        {
            Scales.Add(scale);
        }

        public void PushRotation(Vector3 rotation)
        {
            Rotations.Add(rotation);
        }

        public void PushTranslation(Vector3 translation)
        {
            Translations.Add(translation);
        }
    }
}
