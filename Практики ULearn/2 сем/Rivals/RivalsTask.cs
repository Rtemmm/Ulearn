using System.Collections.Generic;

namespace Rivals;

public class RivalsTask
{
	public static IEnumerable<OwnedLocation> AssignOwners(Map map)
	{
		var queue = new Queue<OwnedLocation>();
		var visited = new HashSet<Point>();
		var players = map.Players;

		for (var i = 0; i < players.Length; i++)
			queue.Enqueue(new OwnedLocation(i, players[i], 0));

		while (queue.Count > 0)
		{
			var result = queue.Dequeue();
			var location = result.Location;
			
			if (visited.Contains(location) || !map.InBounds(location) ||
			    map.Maze[location.X, location.Y] != MapCell.Empty)
				continue;

			visited.Add(location);
			yield return result;
			
			EnqueueNewLocation(result, queue);
		}
	}

	private static void EnqueueNewLocation(OwnedLocation location, Queue<OwnedLocation> queue)
	{
		for (var dy = -1; dy <= 1; dy++)
		for (var dx = -1; dx <= 1; dx++)
			if ((dy == 0 || dx == 0) && dy != dx)
			{
				var newLocation = new OwnedLocation(location.Owner,
					new Point(location.Location.X + dx, location.Location.Y + dy),
					location.Distance + 1);
				
				queue.Enqueue(newLocation);
			}
	}
}