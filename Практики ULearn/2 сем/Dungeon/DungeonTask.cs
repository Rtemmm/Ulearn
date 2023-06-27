using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Dungeon;

public class DungeonTask
{
	public static MoveDirection[] FindShortestPath(Map map)
	{
		var exitRoute = BfsTask.FindPaths
			(map, map.Exit, new[] {map.InitialPosition}).FirstOrDefault();
		if (exitRoute == null) 
			return Array.Empty<MoveDirection>();
		
		var moveToExit = exitRoute.ToList();
		if (map.Chests.Any(x=> moveToExit.Contains(x))) 
			return moveToExit.Zip(moveToExit.Skip(1), GetDirection).ToArray();
		
		var routeStartToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
		var routeExitToChests = BfsTask.FindPaths(map, map.Exit, map.Chests).DefaultIfEmpty();
		return routeStartToChests.FirstOrDefault() == null 
			? moveToExit.Zip(moveToExit.Skip(1), GetDirection).ToArray() 
			: GetMove(GetRouteStartToExit(routeStartToChests, routeExitToChests));
	}

	private static MoveDirection GetDirection(Point start, Point finish)
	{
		var d = finish - start;
		return d switch
		{
			(1, 0) => MoveDirection.Right,
			(-1, 0) => MoveDirection.Left,
			(0, 1) => MoveDirection.Down,
			(0, -1) => MoveDirection.Up,
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	private static Tuple<List<Point>, List<Point>> GetRouteStartToExit
		(IEnumerable<SinglyLinkedList<Point>> routeStartToChests, IEnumerable<SinglyLinkedList<Point>> routeExitToChests)
	{
		return routeStartToChests
			.Join(routeExitToChests, f => f.Value, s => s.Value, (f, s) =>
				new
				{
					Length = f.Length + s.Length,
					listFinish = f.ToList(),
					listStart = s.ToList()
				}).OrderBy(l => l.Length)
			.Select(v => Tuple.Create(v.listFinish, v.listStart)).First();
	}
	
	private static MoveDirection[] GetMove(Tuple<List<Point>,List<Point>>? listsTuple)
	{
		listsTuple?.Item1.Reverse();
		listsTuple?.Item1.AddRange(listsTuple.Item2.Skip(1));
		
		return listsTuple?.Item1.Zip(listsTuple.Item1.Skip(1), GetDirection).ToArray() ?? Array.Empty<MoveDirection>();
	}
}