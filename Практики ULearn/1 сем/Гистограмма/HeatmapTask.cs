using System;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            var days = new string[30];
            var months = new string[12];
            var heatMap = new double[30, 12];

            for (int i = 0; i < days.Length; i++)
                days[i] = (i + 3).ToString();

            for (int i = 0; i < months.Length; i++)
                months[i] = (i + 1).ToString();

            foreach (var x in names)
                if (x.BirthDate.Day != 1)
                    heatMap[x.BirthDate.Day - 2, x.BirthDate.Month - 1]++;
            


            return new HeatmapData(
                "Пример карты интенсивностей",
                heatMap, 
                days, 
                months);
        }
    }
}