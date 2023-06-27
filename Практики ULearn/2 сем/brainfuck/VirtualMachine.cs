using System;
using System.Collections.Generic;

namespace func.brainfuck;

public class VirtualMachine : IVirtualMachine
{
	public string Instructions { get; }
	public int InstructionPointer { get; set; }
	public byte[] Memory { get; }
	public int MemoryPointer { get; set; }

	private readonly Dictionary<char, Action<IVirtualMachine>> _actionDictionary;

	public VirtualMachine(string program, int memorySize)
	{
		Instructions = program;
		Memory = new byte[memorySize];
		_actionDictionary = new Dictionary<char, Action<IVirtualMachine>>(program.Length);
	}

	public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
	{
		if (!_actionDictionary.ContainsKey(symbol))
			_actionDictionary.Add(symbol, execute);
	}

	public void Run()
	{
		while (InstructionPointer < Instructions.Length)
		{
			if (_actionDictionary.ContainsKey(Instructions[InstructionPointer]))
			{
				var action = _actionDictionary[Instructions[InstructionPointer]];
				action(this);
			}

			InstructionPointer++;
		}
	}
}