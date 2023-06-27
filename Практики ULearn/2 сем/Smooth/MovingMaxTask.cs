using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
	{
		var pointQueue = new Queue<double>();
		var maxesList = new LinkedList<double>();

		foreach (var point in data)
		{
			pointQueue.Enqueue(point.OriginalY);
			
			if (pointQueue.Count > windowWidth && pointQueue.Dequeue() == maxesList.First.Value)
				maxesList.RemoveFirst();
			
			while (maxesList.Count > 0 && point.OriginalY > maxesList.Last.Value)
				maxesList.RemoveLast();

			maxesList.AddLast(point.OriginalY);

			yield return point.WithMaxY(maxesList.First.Value);
		}
	}
}