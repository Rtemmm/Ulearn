using System;
using System.Collections.Generic;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
    public static List<string> Calculate(List<string> first, List<string> second)
    {
        var optimizationTable = CreateOptimizationTable(first, second);
        
        return GetAnswer(optimizationTable, first, second);
    }

    private static int[,] CreateOptimizationTable(IReadOnlyList<string> first, IReadOnlyList<string> second)
    {
        var opt = new int[first.Count+1, second.Count+1];
        
        for (var i = first.Count-1; i >= 0; i--)
        for (var j = second.Count-1; j >= 0; j--)
        {
            if (first[i ] == second[j ])
                opt[i, j] = opt[i +1, j+1] + 1;
            else
                opt[i, j] = Math.Max(opt[i+1, j], opt[i, j+1]);
        }
        
        return opt;
    }

    private static List<string> GetAnswer
        (int[,] optimizationTable, IReadOnlyList<string> first, IReadOnlyList<string> second)
    {
        var answer = new List<string>();
        
        var i = 0;
        var j = 0;

        while (i < first.Count && j < second.Count)
        {
            if (first[i] == second[j])
            {
                answer.Add(first[i]);
                i++;
                j++;
            }
            else if (optimizationTable[i, j] == optimizationTable[i + 1, j])
                i++;
            else 
                j++;
        }

        return answer;
    }
}