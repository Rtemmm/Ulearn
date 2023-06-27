using System;
using System.Linq;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            var wristX = x + Math.Cos(Math.PI - alpha) * Manipulator.Palm;
            var wristY = y + Math.Sin(Math.PI - alpha) * Manipulator.Palm;
            var shoulderToWrist = Math.Sqrt(wristX * wristX + wristY * wristY);

            var shoulder = TriangleTask.GetABAngle(Manipulator.UpperArm, shoulderToWrist, Manipulator.Forearm)
                           + Math.Atan2(wristY, wristX);
            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, shoulderToWrist);
            var wrist = -alpha - shoulder - elbow;

            var angles = new[] { shoulder, elbow, wrist };

            if (angles.Contains(double.NaN))
                return new[] { double.NaN, double.NaN, double.NaN };

            return angles;
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            var expectedAngles = new double[3] { double.NaN, double.NaN, double.NaN };
            var max = Manipulator.Forearm + Manipulator.UpperArm + Manipulator.Palm;
            var min = -(Manipulator.Forearm + Manipulator.UpperArm + Manipulator.Palm);

            for (var i = 0; i < 100; i++)
            {
                var x = new Random().NextDouble() * (max - min) + min;
                var y = new Random().NextDouble() * (max - min) + min;
                var alpha = new Random().NextDouble() * (2 * Math.PI) - Math.PI;

                var actualAngles = ManipulatorTask.MoveManipulatorTo(x, y, alpha);
                
                if (actualAngles.Any(u => u == double.NaN))
                {
                    CollectionAssert.AreEqual(expectedAngles, actualAngles);
                    continue;
                }

                var actualXY = AnglesToCoordinatesTask.GetJointPositions(actualAngles[0], actualAngles[1],
                    actualAngles[2]);
                Assert.AreEqual((float)x, actualXY[2].X, 1e-5);
                Assert.AreEqual((float)y, actualXY[2].Y, 1e-5);
            }
        }
    }
}