using System.Collections.Generic;

namespace func.brainfuck;

public static class BrainfuckLoopCommands
{
	public static void RegisterTo(IVirtualMachine vm)
	{
		var loopDictionary = GetLoopDictionary(vm);
			
		vm.RegisterCommand('[', b =>
		{
			if (b.Memory[b.MemoryPointer] == 0)
				b.InstructionPointer = loopDictionary[b.InstructionPointer];
		});
			
		vm.RegisterCommand(']', b =>
		{
			if (b.Memory[b.MemoryPointer] != 0)	
				b.InstructionPointer = loopDictionary[b.InstructionPointer];
		});
	}

	private static Dictionary<int, int> GetLoopDictionary(IVirtualMachine vm)
	{
		var loopDictionary = new Dictionary<int, int>();
		var instructionStack = new Stack<int>();
		
		for (var i = 0; i < vm.Instructions.Length; i++)
			switch (vm.Instructions[i])
			{
				case '[':
					instructionStack.Push(i);
					break;
					
				case ']':
					loopDictionary[i] = instructionStack.Peek();
					loopDictionary[instructionStack.Pop()] = i;
					break;
			}

		return loopDictionary;
	}
}