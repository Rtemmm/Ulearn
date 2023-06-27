using System.Linq;

namespace Recognizer
{
    public static class ThresholdFilterTask
	{
        enum Color
		{
			Black,
			White
        }

		public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
		{
			var rows = original.GetLength(0);
			var columns = original.GetLength(1);

			var thresholdValue = GetThresholdValue(original, whitePixelsFraction);

			for (int x = 0; x < rows; x++)
				for (int y = 0; y < columns; y++)
					original[x, y] = original[x, y] >= thresholdValue ?
			   (double)Color.White : (double)Color.Black;

			return original;
		}

		public static double GetThresholdValue(double[,] original, double whitePixelsFraction)
        {
			var thresholdCount = (int)(whitePixelsFraction * original.Length);

			if (thresholdCount > 0 && thresholdCount <= original.Length)
            {
                var brightPixelsFirst = original.Cast<double>().OrderByDescending(x => x);
                return brightPixelsFirst.ElementAt(thresholdCount - 1);
            }

            return int.MaxValue;
        }
	}
}