using System;
using System.Collections.Generic;

namespace Clones;

public class Stack
{
	public StackItem LastItem { get; private set; }

	public Stack()
	{
	}

	public Stack(Stack stack)
	{
		LastItem = stack.LastItem;
	}

	public void Push(string value)
	{
		LastItem = new StackItem(value, LastItem);
	}

	public string Pop()
	{
		if (LastItem == null)
			throw new NotImplementedException();

		var poppedValue = LastItem.value;
		LastItem = LastItem.previousItem;

		return poppedValue;
	}
}

public record StackItem(string value, StackItem previousItem);

public class Clone
{
	private readonly Stack _addedProgram;
	private readonly Stack _removedProgram;

	public Clone()
	{
		_addedProgram = new Stack();
		_removedProgram = new Stack();
	}

	public Clone(Clone clone)
	{
		_addedProgram = new Stack(clone._addedProgram);
		_removedProgram = new Stack(clone._removedProgram);
	}

	public void Learn(string program) => _addedProgram.Push(program);

	public void Rollback() => _removedProgram.Push(_addedProgram.Pop());

	public void Relearn() => _addedProgram.Push(_removedProgram.Pop());

	public string Check() =>  _addedProgram.LastItem?.value ?? "basic";
}

public class CloneVersionSystem : ICloneVersionSystem
{
	private readonly List<Clone> _listClone;

	public CloneVersionSystem()
	{
		_listClone = new List<Clone> { new Clone() };
	}

	public string Execute(string query)
	{
		var splitQuery = query.Split();
		
		var command = splitQuery[0];
		var index = int.Parse(splitQuery[1]) - 1;

		switch (command)
		{
			case "learn":
				var program = splitQuery[2];
				_listClone[index].Learn(program);
				break;
			
			case "rollback":
				_listClone[index].Rollback();
				break;
			
			case "relearn":
				_listClone[index].Relearn();
				break;
			
			case "clone":
				_listClone.Add(new Clone(_listClone[index]));
				break;
			
			case "check":
				return _listClone[index].Check();
		}

		return null;
	}
	
}