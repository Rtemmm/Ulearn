using System.Numerics;

namespace Tickets;

public class TicketsTask
{
    public static BigInteger Solve(int halfLen, int totalSum)
    {
        if (totalSum % 2 != 0)
            return 0;
        
        var halfSum = totalSum / 2;
        var luckyTicketsCount = new BigInteger[totalSum + 1, halfLen + 1];

        var columns = luckyTicketsCount.GetLength(0);
        var rows = luckyTicketsCount.GetLength(1);
        
        for (var i = 0; i < rows; i++)
            luckyTicketsCount[0, i] = 1;
        
        for (var i = 1; i < columns; i++)
        for (var j = 1; j < rows; j++)
        for (var k = i; k >= (i <= 9 ? 0 : i - 9); k--)
            luckyTicketsCount[i, j] += luckyTicketsCount[k, j - 1];
        
        return luckyTicketsCount[halfSum, halfLen] * luckyTicketsCount[halfSum, halfLen];
    }
}