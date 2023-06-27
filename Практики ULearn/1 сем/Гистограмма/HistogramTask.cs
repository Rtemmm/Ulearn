using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var days = new string[31];
            var birthsByDay = new double[31];

            for (int i = 0; i < days.Length; i++)
                days[i] = (i + 1).ToString();

            foreach (var x in names)
                if (x.Name == name && x.BirthDate.Day != 1)
                    birthsByDay[x.BirthDate.Day - 1] += 1;
            
            return new HistogramData(
                string.Format("Рождаемость людей с именем '{0}'", name), 
                days, 
                birthsByDay);
        }
    }
}