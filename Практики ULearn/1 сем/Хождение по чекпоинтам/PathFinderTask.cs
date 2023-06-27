using System;
using System.Drawing;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            var length = checkpoints.Length;

            var bestOrder = new int[length];
            var currentOrder = new int[length];

            var minDistance = double.MaxValue;
            var currentDistance = 0.0;

            MakeTrivialPermutation(1, length, currentDistance, checkpoints, currentOrder, bestOrder, ref minDistance);

            return bestOrder;
        }

        private static void MakeTrivialPermutation(int position, int length, double currentDistance,
            Point[] checkpoints, int[] currentOrder, int[] bestOrder, ref double minDistance)
        {
            if (position == length)
            {
                currentDistance = checkpoints.GetPathLength(currentOrder);

                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    Array.Copy(currentOrder, bestOrder, length);
                }
                return;
            }

            for (int i = 0; i < currentOrder.Length; i++)
            {
                var index = Array.IndexOf(currentOrder, i, 0, position);

                if (index == -1)
                {
                    currentOrder[position] = i;
                    MakeTrivialPermutation(position + 1, length, currentDistance, checkpoints,
                        currentOrder, bestOrder, ref minDistance);
                }
            }
        }
    }
}