namespace Pluralize
{
	public static class PluralizeTask
	{
		public static string PluralizeRubles(int count)
		{
			///заканчивается на 1 - рубль
			///заканчивается на 2, 3, 4 - рубля
			///заканчивается на 5, 6, 7, 8, 9, 0 - рублей
			
			int residual = count % 100;

			if (count % 100 > 10 && count % 100 < 20)
				return "рублей";

			if (residual == 1)
				return "рубль";

			else if (residual == 2 || residual == 3 || residual == 4)
				return "рубля";

			else
				return "рублей";
		}
	}
}