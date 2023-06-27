using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
	public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
	{
		return visits
			.OrderBy(x => x.DateTime)
			.GroupBy(x => x.UserId)
			.SelectMany(x => x.Bigrams()
				.Where(c => c.First.SlideType == slideType))
			.Select(x => x.Second.DateTime.Subtract(x.First.DateTime).TotalMinutes)
			.Where(x => x is >= 1 and <= 120)
			.DefaultIfEmpty()
			.Median();
	}
}