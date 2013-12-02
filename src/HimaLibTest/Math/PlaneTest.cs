using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace HimaLib.Math
{
    public class TestDataTypeDistanceNormal
    {
        public float Distance { get; set; }
        public Vector3 Normal { get; set; }
    }

    public class TestDataTypePoints
    {
        public Vector3 Point1 { get; set; }
        public Vector3 Point2 { get; set; }
        public Vector3 Point3 { get; set; }
    }

    [TestFixture]
    public class PlaneTest
    {
        static readonly double Delta = 0.000001;

        static TestDataTypeDistanceNormal[] TestDataDistanceNormal =
        {
            new TestDataTypeDistanceNormal()
            {
                Distance = 5.38f,
                Normal = new Vector3(-4.3f, 0.88f, 10.3f),
            },
            new TestDataTypeDistanceNormal()
            {
                Distance = -0.444f,
                Normal = new Vector3(24.5f, -10.03f, 3.3333f),
            },
            new TestDataTypeDistanceNormal()
            {
                Distance = 70.88f,
                Normal = new Vector3(-4378.0003f, 1.11f, -0.0004425f),
            },
        };

        static TestDataTypePoints[] TestDataPoints = 
        {
            new TestDataTypePoints()
            {
                Point1 = new Vector3(4, 5, 6),
                Point2 = new Vector3(-2, -10, 16),
                Point3 = new Vector3(9, 3, 0),
            },
            new TestDataTypePoints()
            {
                Point1 = new Vector3(0.15f, -4.22f, 3.1f),
                Point2 = new Vector3(0.004f, 5.22f, -11.44f),
                Point3 = new Vector3(-7.33f, 20.5f, -200.5f),
            },
            new TestDataTypePoints()
            {
                Point1 = new Vector3(-0.00525f, 0.002f, -0.02f),
                Point2 = new Vector3(-2.8f, 14.3f, 3.044f),
                Point3 = new Vector3(51.44f, 56490.5f, -20522.0f),
            },
        };

        [TestCaseSource("TestDataDistanceNormal")]
        public void XnaCompatible(TestDataTypeDistanceNormal data)
        {
            var himaPlane = new HimaLib.Math.Plane(data.Normal, data.Distance);

            var xnaPlane = new Microsoft.Xna.Framework.Plane(
                MathUtilXna.ToXnaVector(data.Normal), data.Distance);

            Assert.AreEqual(xnaPlane.Normal.X, himaPlane.Normal.X, Delta);
            Assert.AreEqual(xnaPlane.Normal.Y, himaPlane.Normal.Y, Delta);
            Assert.AreEqual(xnaPlane.Normal.Z, himaPlane.Normal.Z, Delta);
            Assert.AreEqual(xnaPlane.D, himaPlane.D, Delta);
        }

        [TestCaseSource("TestDataPoints")]
        public void XnaCompatible(TestDataTypePoints data)
        {
            var himaPlane = new HimaLib.Math.Plane(data.Point1, data.Point2, data.Point3);

            var xnaPlane = new Microsoft.Xna.Framework.Plane(
                MathUtilXna.ToXnaVector(data.Point1),
                MathUtilXna.ToXnaVector(data.Point2),
                MathUtilXna.ToXnaVector(data.Point3));

            Assert.AreEqual(xnaPlane.Normal.X, himaPlane.Normal.X, Delta);
            Assert.AreEqual(xnaPlane.Normal.Y, himaPlane.Normal.Y, Delta);
            Assert.AreEqual(xnaPlane.Normal.Z, himaPlane.Normal.Z, Delta);
            Assert.AreEqual(xnaPlane.D, himaPlane.D, Delta);
        }
    }
}
