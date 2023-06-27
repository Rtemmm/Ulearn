using System;
using System.Collections.Generic;

namespace Recognizer
{
    internal static class MedianFilterTask
	{
		public static double[,] MedianFilter(double[,] original)
		{
			var rows = original.GetLength(0);
			var columns = original.GetLength(1);

			var result = new double[rows, columns];

			for (int x = 0; x < rows; x++)
			    for (int y = 0; y < columns; y++)
                    result[x, y] = GetMedian(GetNeighbourPixels(x, y, rows, columns, original));
                
			return result;
		}

        public static double GetMedian(List<double> neighbourPixels)
        {
            neighbourPixels.Sort();

            if (neighbourPixels.Count % 2 == 0)
            {
                var middle1 = neighbourPixels[neighbourPixels.Count / 2 - 1];
                var middle2 = neighbourPixels[neighbourPixels.Count / 2];
                return (middle1 + middle2) / 2;
            }
            
            return neighbourPixels[neighbourPixels.Count / 2];
        }

        public static List<double> GetNeighbourPixels(int x, int y, int rows, int columns, double[,] original)
        {
            var neighbourPixels = new List<double>();

            var startX = x == 0 ? x : x - 1;
            var startY = y == 0 ? y : y - 1;
            var endX = x == rows - 1 ? x : x + 1;
            var endY = y == columns - 1 ? y : y + 1;

            for (int i = startX; i <= endX; i++)
                for (int j = startY; j <= endY; j++)
                    neighbourPixels.Add(original[i, j]);

            return neighbourPixels;
        }  
    }
}