using System.Drawing;
using System;

namespace Fractals
{
	internal static class DragonFractalTask
	{
		public static (double, double) GetFirstTransformation(double angle1, double angle2, double x, double y)
        {
			return ((x * Math.Cos(angle1) - y * Math.Sin(angle1)) / Math.Sqrt(2),
					(x * Math.Sin(angle1) + y * Math.Cos(angle1)) / Math.Sqrt(2));
		}

		public static (double, double) GetSecondTransformation(double angle1, double angle2, double x, double y)
        {
			return ((x * Math.Cos(angle2) - y * Math.Sin(angle2)) / Math.Sqrt(2) + 1,
					(x * Math.Sin(angle2) + y * Math.Cos(angle2)) / Math.Sqrt(2));
		}

		public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
		{
			var random = new Random(seed);

			var x = 1.0;
			var y = 0.0;

			var angle1 = Math.PI * 45 / 180;
			var angle2 = Math.PI * 135 / 180;

			for (int i = 0; i < iterationsCount; i++)
            {
				if (random.Next(2) == 0)
					(x, y) = GetFirstTransformation(angle1, angle2, x, y);
				else 
					(x, y) = GetSecondTransformation(angle1, angle2, x, y);

				pixels.SetPixel(x, y);
			}
		}
	}
}