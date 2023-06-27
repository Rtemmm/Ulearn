using System;
using System.Linq;

namespace GaussAlgorithm;

public class Solver
{
	public static double[] Solve(double[][] matrix, double[] freeMembers)
	{
		var n = matrix.Length;
		var m = matrix[0].Length;

		var expandedMatrix = GetExpandedMatrix(matrix, freeMembers, n, m);
		var usedRows = Enumerable.Repeat(-1, n).ToArray();

		for (var col = 0; col < m; col++)
		{
			var found = false;
			for (var row = 0; row < n; row++)
			{
				if (expandedMatrix[row][col] == 0 || usedRows[row] != -1)
					continue;

				usedRows[row] = col;
				found = true;

				var x = expandedMatrix[row][col];

				for (var j = 0; j <= m; j++)
					expandedMatrix[row][j] /= x;

				for (var i = 0; i < n; i++)
				{
					if (i == row)
						continue;

					var y = expandedMatrix[i][col];

					for (var j = 0; j <= m; j++)
						expandedMatrix[i][j] -= y * expandedMatrix[row][j];
				}

				break;
			}

			if (!found)
				usedRows[col] = col;
		}
		
		if (CheckSolution(expandedMatrix, n, m))
			throw new NoSolutionException(matrix, freeMembers, expandedMatrix);

		return GetSolution(expandedMatrix, usedRows, n, m);
	}

	private static double[][] GetExpandedMatrix(double[][] matrix, double[] freeMembers, int n, int m)
	{
		var expandedMatrix = new double[n][];

		for (var i = 0; i < n; i++)
		{
			expandedMatrix[i] = new double[m + 1];

			for (var j = 0; j < m; j++)
				expandedMatrix[i][j] = matrix[i][j];

			expandedMatrix[i][m] = freeMembers[i];
		}

		return expandedMatrix;
	}
	
	private static bool CheckSolution(double[][] expandedMatrix, int n, int m)
	{
		for (var i = 0; i < n; i++)
		{
			var allZero = true;
			
			for (var j = 0; j < m; j++)
			{
				if (expandedMatrix[i][j] == 0) 
					continue;
				
				allZero = false;
				break;
			}

			if (allZero && expandedMatrix[i][m] != 0)
				return true;
		}

		return false;
	}

	private static double[] GetSolution(double[][] expandedMatrix, int[] usedRows, int n, int m)
	{
		var solution = new double[m];

		for (var i = 0; i < n; i++)
		{
			var col = usedRows[i];

			if (col != -1)
				solution[col] = expandedMatrix[i][m];
		}

		return solution;
	}
}
