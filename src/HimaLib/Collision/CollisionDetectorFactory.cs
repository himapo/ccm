using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Collision
{
    public class CollisionDetectorFactory
    {
        public ICollisionDetector Create(ICollisionPrimitive paramA, ICollisionPrimitive paramB)
        {
            switch (paramA.Shape)
            {
                case CollisionShape.Sphere:
                    switch (paramB.Shape)
                    {
                        case CollisionShape.Sphere:
                            var result = new SphereSphereCollisionDetector();
                            result.ParamA = paramA as SphereCollisionPrimitive;
                            result.ParamB = paramB as SphereCollisionPrimitive;
                            return result;
                        default:
                            break;
                    }
                    break;
                case CollisionShape.Cylinder:
                    switch (paramB.Shape)
                    {
                        case CollisionShape.Cylinder:
                            var result = new CylinderCylinderCpollisionDetector();
                            result.ParamA = paramA as CylinderCollisionPrimitive;
                            result.ParamB = paramB as CylinderCollisionPrimitive;
                            return result;
                    }
                    break;
                default:
                    break;
            }

            return null;
        }
    }
}
