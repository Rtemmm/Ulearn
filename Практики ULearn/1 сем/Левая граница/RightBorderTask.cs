using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete
{
    public class RightBorderTask
    {
        /// <returns>
        /// Возвращает индекс правой границы. 
        /// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
        /// Если такого нет, то возвращает items.Length
        /// </returns>
        /// <remarks>
        /// Функция должна быть НЕ рекурсивной
        /// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
        /// </remarks>
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            while (left + 1 != right)
            {
                var middle = left + (right - left) / 2;
                var comparisonResult = string.Compare(phrases[middle].Substring
                    (0, Math.Min(prefix.Length, phrases[middle].Length)), prefix, StringComparison.OrdinalIgnoreCase);

                if (comparisonResult <= 0)
                    left = middle;
                else
                    right = middle;
            }
            return right;
        }
    }
}