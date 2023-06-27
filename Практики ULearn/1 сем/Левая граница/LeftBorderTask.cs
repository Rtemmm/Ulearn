using System;
using System.Collections.Generic;

namespace Autocomplete
{
    public class LeftBorderTask
    {
        public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            if (left + 1 >= right)
                return left;

            var middle = left + (right - left) / 2;
            
            var comparisonResult = string.Compare(phrases[middle], prefix, StringComparison.OrdinalIgnoreCase);
            var startsWithPrefix = phrases[middle].StartsWith(prefix, StringComparison.OrdinalIgnoreCase);

            if (comparisonResult > 0 || startsWithPrefix)
                return GetLeftBorderIndex(phrases, prefix, left, middle);

            return GetLeftBorderIndex(phrases, prefix, middle, right);
        }
    }
}