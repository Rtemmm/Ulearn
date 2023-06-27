using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
	/// <summary>
	/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
	/// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
	/// </summary>
	/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
	public static double Median(this IEnumerable<double> items)
	{
		var itemsList = items
			.OrderBy(x => x)
			.ToList();

		if (itemsList.Count == 0)
			throw new InvalidOperationException();

		return itemsList.Count % 2 == 0
			? (itemsList[itemsList.Count / 2] + itemsList[itemsList.Count / 2 - 1]) / 2
			: itemsList[itemsList.Count / 2];
	}

	/// <returns>
	/// Возвращает последовательность, состоящую из пар соседних элементов.
	/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
	/// </returns>
	public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
	{
		var isFirst = true;
		var previousItem = default(T);
		
		foreach (var item in items)
		{
			if (isFirst)
			{
				isFirst = false;
				previousItem = item;
				continue;
			}
			
			yield return (previousItem, item);

			previousItem = item;
		}
	}
}