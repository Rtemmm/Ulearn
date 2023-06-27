using System.Collections.Generic;
using System.Linq;

using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    private List<Point> bestWay = new ();
    private HashSet<Point> bestFoundedChests = new ();
  
    public List<Point> FindPathToCompleteGoal(State state)
    {
        FindBestWay(state, state.Position, state.InitialEnergy, 
            new HashSet<Point>(), state.Chests, new List<Point>());
        
        return bestWay;
    }

    private void FindBestWay
    (State state, Point position, int energy, 
        HashSet<Point> foundedChests, ICollection<Point> chests, List<Point> way)
    {
        if (bestFoundedChests.Count == state.Chests.Count)
            return;
        if (foundedChests.Count > bestFoundedChests.Count)
        {
            bestFoundedChests = foundedChests;
            bestWay = way;
        }
        var paths = new DijkstraPathFinder().GetPathsByDijkstra(state, position, chests);
        foreach (var path in paths)
        {
            if (energy - path.Cost < 0)
                return;
            var newWay = new List<Point>(way);
            newWay.AddRange(path.Path.Skip(1));
            var tempChests = new HashSet<Point>(chests);
            var tempFoundedChests = new HashSet<Point>(foundedChests);
            tempChests.Remove(path.End);
            tempFoundedChests.Add(path.End);
            FindBestWay(state, path.End, energy - path.Cost, tempFoundedChests, tempChests, newWay);
        }
    }
}