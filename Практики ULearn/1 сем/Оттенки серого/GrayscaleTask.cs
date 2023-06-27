namespace Recognizer
{
	public static class GrayscaleTask
	{
		public static double[,] ToGrayscale(Pixel[,] original)
		{
			var rows = original.GetLength(0);
			var columns = original.GetLength(1);

			var grayscale = new double[rows, columns];

			for (int i = 0; i < rows; i++)
				for (int j = 0; j < columns; j++)
				{
					var pixel = original[i, j];
					grayscale[i, j] = (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B) / 255;
				}

			return grayscale;
		}
	}
}