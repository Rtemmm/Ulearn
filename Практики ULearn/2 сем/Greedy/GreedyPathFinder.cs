using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
	public List<Point> FindPathToCompleteGoal(State state)
	{
		var energy = state.InitialEnergy;
		var currentPosition = state.Position;
		var chests = state.Chests;
		var pathToGoal = new List<Point>();
		
		for (var i = 0; i < state.Goal; i++)
		{
			var pathToChest = new DijkstraPathFinder().GetPathsByDijkstra(state, currentPosition, chests).FirstOrDefault();

			if (pathToChest == null)
				return new List<Point>();

			energy -= pathToChest.Cost;
			currentPosition = pathToChest.End;

			if (energy < 0)
				return new List<Point>();

			chests.Remove(currentPosition);
			pathToGoal.AddRange(pathToChest.Path.Skip(1));
		}
		return pathToGoal;
	}
}