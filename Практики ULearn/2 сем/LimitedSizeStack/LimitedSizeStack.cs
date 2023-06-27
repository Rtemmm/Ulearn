using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
	private readonly LinkedList<T> stack = new();
	private readonly int undoLimit;

	public LimitedSizeStack(int undoLimit)
	{
		this.undoLimit = undoLimit;
	}

	public void Push(T item)
	{
		if (undoLimit == 0)
			return;
		
		if (stack.Count == undoLimit)
			stack.RemoveFirst();

		stack.AddLast(item);
		
	}

	public T Pop()
	{
		var stackLastValue = stack.Last.Value;
		stack.RemoveLast();

		return stackLastValue;
	}

	public int Count => stack.Count;
}
