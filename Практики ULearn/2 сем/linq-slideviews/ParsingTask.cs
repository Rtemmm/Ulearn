using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace linq_slideviews;

public class ParsingTask
{
	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
	/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
	/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
	public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
	{
		return lines
			.Skip(1)
			.Where(x => Regex.IsMatch(x, @"\d*;\D+;\D+"))
			.Select(x => x.Split(";"))
			.Where(x =>
			{
				var isInt = int.TryParse(x[0], out _);
				var isEnum = Enum.TryParse<SlideType>(char.ToUpper(x[1][0]) + x[1][1..], out _);
				
				return isInt && isEnum;
			})
			.ToDictionary(x => int.Parse(x[0]), x => new SlideRecord(int.Parse(x[0]), 
				Enum.Parse<SlideType>(char.ToUpper(x[1][0]) + x[1][1..]), x[2]));
	}

	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
	/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
	/// Такой словарь можно получить методом ParseSlideRecords</param>
	/// <returns>Список информации о посещениях</returns>
	/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
	public static IEnumerable<VisitRecord> ParseVisitRecords(
		IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
	{
		return lines
			.Skip(1)
			.Select(x => x.Split(";"))
			.Where(x => CheckVisitRecord(x, slides)
				? true
				: throw new FormatException($"Wrong line [{string.Join(';', x)}]"))
			.Select(x => new VisitRecord(int.Parse(x[0]),
				int.Parse(x[1]),
				DateTime.ParseExact($"{x[2]} {x[3]}", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
				slides[int.Parse(x[1])].SlideType));
	}

	private static bool CheckVisitRecord(IReadOnlyList<string> x, IDictionary<int, SlideRecord> slides)
	{
		if (x.Count != 4)
			return false;
				
		var isCorrectUserId = int.TryParse(x[0], out _);
		var isCorrectSlideId = int.TryParse(x[1], out _);
		var isCorrectDateTime = DateTime.TryParseExact($"{x[2]} {x[3]}", "yyyy-MM-dd HH:mm:ss",
			CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

		return isCorrectUserId && isCorrectDateTime && isCorrectSlideId && slides.ContainsKey(int.Parse(x[1]));
	}
}