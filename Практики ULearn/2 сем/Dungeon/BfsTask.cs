using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
    {
        var queue = new Queue<SinglyLinkedList<Point>>();
        var visited = new HashSet<Point> {start};
        var possibleDirections = Walker.PossibleDirections.Select(p => new Point(p.X, p.Y));
        var chest = new HashSet<Point>(chests);
        queue.Enqueue(new SinglyLinkedList<Point>(start));
        
        while (queue.Count != 0)
        {
            var result = queue.Dequeue();
            var point = result.Value;
            
            if (!map.InBounds(point) || map.Dungeon[point.X,point.Y] != MapCell.Empty) continue;
            if (chest.Contains(point)) yield return result;

            foreach (var direction in possibleDirections)
            {
                var nextPoint = new Point(point.X + direction.X, point.Y + direction.Y);
                if (visited.Contains(nextPoint)) continue;
                queue.Enqueue(new SinglyLinkedList<Point>(nextPoint, result));
                visited.Add(nextPoint);
            }
        }
    }
}