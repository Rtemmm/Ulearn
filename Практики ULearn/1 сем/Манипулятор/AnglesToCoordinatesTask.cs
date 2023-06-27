using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowPos = GetPointF(Manipulator.UpperArm, shoulder, 0, 0);
            var wristPos = GetPointF(Manipulator.Forearm, shoulder + elbow + Math.PI, elbowPos.X, elbowPos.Y);
            var palmEndPos = GetPointF(Manipulator.Palm, shoulder + elbow + wrist + 2*Math.PI, 
                wristPos.X, wristPos.Y);
            
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
        
        public static PointF GetPointF(float segment, double angle, float lastPointX, float lastPointY)
        {
            var x = segment * Math.Cos(angle) + lastPointX;
            var y = segment * Math.Sin(angle) + lastPointY;

            return new PointF((float)x, (float)y);
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI / 2, 60, 270)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI / 2, 120, 90)]
        [TestCase(Math.PI, Math.PI / 2, Math.PI, -150, 180)]
        [TestCase(Math.PI, Math.PI, Math.PI / 2, -270, 60)]
        [TestCase(Math.PI, Math.PI / 2, Math.PI / 2, -90, 120)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            AssertPointsAreEqual(new PointF((float)palmEndX, (float)palmEndY), joints[2]);
            
            Assert.AreEqual(Manipulator.UpperArm, GetDistanceBetweenPoints
                (new PointF(0, 0), joints[0]));
            Assert.AreEqual(Manipulator.Forearm, GetDistanceBetweenPoints
                (joints[0], joints[1]));
            Assert.AreEqual(Manipulator.Palm, GetDistanceBetweenPoints
                (joints[1], joints[2]));
        }
        
        public static void AssertPointsAreEqual(PointF point1, PointF point2)
        {
            Assert.AreEqual(point1.X, point2.X, 1e-5, "palm endX");
            Assert.AreEqual(point1.Y, point2.Y, 1e-5, "palm endY");
        }
        
        public static float GetDistanceBetweenPoints(PointF point1, PointF point2)
        {
            return (float)Math.Sqrt((point2.X - point1.X) * (point2.X - point1.X) +
                                    (point2.Y - point1.Y) * (point2.Y - point1.Y));
        }
    }
}