using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
	public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{
		var pointQueue = new Queue<double>();
		var sum = 0.0;

		foreach (var point in data)
		{
			if (pointQueue.Count == windowWidth)
				sum -= pointQueue.Dequeue();
			
			pointQueue.Enqueue(point.OriginalY);
			sum += point.OriginalY;
			var currentPoint = new DataPoint(point).WithAvgSmoothedY(sum / pointQueue.Count);

			yield return currentPoint;
		}
	}
}