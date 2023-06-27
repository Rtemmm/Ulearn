using System;

namespace DistanceTask
{
	public static class DistanceTask
	{
		// Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
		public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
		{
			var sideAC = Math.Sqrt(Math.Pow((ay - y), 2) + Math.Pow((ax - x), 2));
			var sideBC = Math.Sqrt(Math.Pow((by - y), 2) + Math.Pow((bx - x), 2));
			var sideAB = Math.Sqrt(Math.Pow((by - ay), 2) + Math.Pow((bx - ax), 2));

			if (ax == x && ay == y || bx == x && by == y)
				return 0;

			if (sideAB == 0)
				return sideAC;

			if (Math.Pow(sideAC, 2) > Math.Pow(sideBC, 2) + Math.Pow(sideAB, 2) ||
				Math.Pow(sideBC, 2) > Math.Pow(sideAC, 2) + Math.Pow(sideAB, 2))
				return Math.Min(sideAC, sideBC);

			var p = (sideAB + sideAC + sideBC) / 2;

			return 2 * Math.Sqrt(p * (p - sideAB) * (p - sideBC) * (p - sideAC)) / sideAB;
		}
	}
}