using System.Collections.Generic;

namespace rocket_bot;

public class Channel<T> where T : class
{
	private static List<T> list;
	private T? lastItem;

	public Channel()
	{
		list = new List<T>();
		lastItem = null;
	}
	
	public T this[int index]
	{
		get
		{
			lock (list)
			{
				return index < Count && index >= 0 ? list[index] : null;
			}
		}
		set
		{
			lock (list)
			{
				if (Count == index)
					list.Add(value);
				else
				{
					list[index] = value;
					list.RemoveRange(index + 1, Count - index - 1);
				}
			}

			lastItem = value;
		}
	}

	public T LastItem() => lastItem;
	
	public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
	{
		if (knownLastItem == lastItem)
		{
			lock (list)
			{
				this[Count] = item;
			}
		}
	}
	
	public int Count
	{
		get
		{
			lock (list)
			{
				return list.Count;
			}
		}
	}
}