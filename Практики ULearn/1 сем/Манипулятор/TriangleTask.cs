using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        public static double GetABAngle(double a, double b, double c)
        {
            return IsTriangleExists(a, b, c) ? Math.Acos((a * a + b * b - c * c) / (2 * a * b)) : double.NaN;
        }

        public static bool IsTriangleExists(double a, double b, double c)
        {
            return !(a > b + c || b > a + c || c > a + b);          
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(99, 4900, 4901, Math.PI / 2)]
        [TestCase(1, 1, 1, Math.PI / 3)] 
        [TestCase(2, 2, 2, Math.PI / 3)]
        [TestCase(0, 0, 0, double.NaN)]
        [TestCase(0, 1, 2, double.NaN)]
        [TestCase(1, 0, 2, double.NaN)]
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            var actualAngle = TriangleTask.GetABAngle(a, b, c);
            Assert.AreEqual(expectedAngle, actualAngle, 3e-16);
        }
    }
}