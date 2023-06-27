using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {     
        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);

            var offset = (int)sx.GetLength(0) / 2;
            var matrixLength = sx.GetLength(0);

            return Convolve(offset, width, height, matrixLength, sx, g);
        }

        public static double[,] Convolve(int offset, int width, int height, int matrixLength, double[,] sx, double[,] g)
        {
            var result = new double[width, height];

            for (int x = offset; x < width - offset; x++)
            {
                for (int y = offset; y < height - offset; y++)
                {
                    var gx = 0.0;
                    var gy = 0.0;

                    for (int i = 0; i < matrixLength; i++)
                        for (int j = 0; j < matrixLength; j++)
                        {
                            gx += sx[i, j] * GetGSurroundings(g, x, y, offset, matrixLength)[i, j];
                            gy += sx[j, i] * GetGSurroundings(g, x, y, offset, matrixLength)[i, j];
                        }
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            }
            return result;
        }

        public static double[,] GetGSurroundings(double[,] g, int x, int y, int offset, int length)
        {
            var gSurroundings = new double[length, length];

            for (int i = -offset; i <= offset; i++)
                for (int j = -offset; j <= offset; j++)
                    gSurroundings[i + offset, j + offset] = g[x + j, y + i];

            return gSurroundings;
        }
    }
}