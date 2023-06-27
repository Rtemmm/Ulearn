using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class DijkstraPathFinder
{
	private record DijkstraData(int Price, Point? Previous);
	public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
	{
		var chests = new HashSet<Point>(targets);
		var visited = new HashSet<Point>();
		var notVisited = new HashSet<Point> { start };
		var track = new Dictionary<Point, DijkstraData> { [start] = new (0, null) };
		while (true)
		{
			 Point? toOpen = null;
			 var bestPrice = int.MaxValue;
			 foreach (var point in notVisited.Where(point => track[point].Price < bestPrice))
			 {
				 bestPrice = track[point].Price;
				 toOpen = point;
			 }
			 if (toOpen == null)
				 yield break;
			 if (chests.Contains(toOpen.Value))
				 yield return GetPathToChest(toOpen, track);
			 FindNeighbours(toOpen, state, track, notVisited);
			 notVisited.Remove(toOpen.Value);
			 visited.Add(toOpen.Value);
		}
	}

	private PathWithCost GetPathToChest(Point? toOpen, IDictionary<Point, DijkstraData> track)
	{
		var end = toOpen;
		var path = new List<Point>();
				 
		while (end != null)
		{
			path.Add(end.Value);
			end = track[end.Value].Previous;
		}

		path.Reverse();
				 
		return new PathWithCost(track[toOpen.Value].Price, path.ToArray());
	}
	
	private void FindNeighbours
		(Point? toOpen, State state, IDictionary<Point, DijkstraData> track, ISet<Point> notVisited)
	{
		foreach (var point in GetNeighbours(toOpen, state))
		{
			var currentPrice = track[toOpen.Value].Price + state.CellCost[point.X, point.Y];

			if (track.ContainsKey(point)) 
				continue;
				 
			track[point] = new DijkstraData(currentPrice, toOpen);
			notVisited.Add(point);
		}
	}
	
	private List<Point> GetNeighbours(Point? point, State state)
	{
		var neighbours = new List<Point>
		{
			new (1, 0),
			new (-1, 0),
			new (0, 1),
			new (0, -1),
		};

		return neighbours
			.Select(p => p + point.Value)
			.Where(p => state.InsideMap(p) && !state.IsWallAt(p))
			.ToList();
	}
}