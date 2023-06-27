using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
		DataPoint previousPoint = null;
		
		foreach (var point in data)
		{
			if (previousPoint == null)
			{
				previousPoint = point.WithExpSmoothedY(point.OriginalY);
				
				yield return previousPoint;
			}
			else
			{
				var expSmoothedY = alpha * point.OriginalY + (1 - alpha) * previousPoint.ExpSmoothedY;
				var currentPoint = point.WithExpSmoothedY(expSmoothedY);
				previousPoint = new DataPoint(currentPoint);
				
				yield return currentPoint;
			}
		}
	}
}