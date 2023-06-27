using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace func.brainfuck;

public class BrainfuckBasicCommands
{
    public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
    {
        vm.RegisterCommand('.', b => { write(Convert.ToChar(b.Memory[b.MemoryPointer])); });
        vm.RegisterCommand(',', b => { b.Memory[b.MemoryPointer] = Convert.ToByte(read()); });
		
        RegisterIncreaseDecreaseMemoryByte(vm);
        RegisterMoveMemoryPointer(vm);
        RegisterChars(vm);
    }

    private static void RegisterIncreaseDecreaseMemoryByte(IVirtualMachine vm)
    {
        vm.RegisterCommand('+', b =>
        {
            var memoryByte = (int)b.Memory[b.MemoryPointer];
            memoryByte += GetSummationValue(memoryByte, byte.MaxValue, byte.MaxValue);
            
            b.Memory[b.MemoryPointer] = (byte)memoryByte;
        });
		
        vm.RegisterCommand('-', b =>
        {
            var memoryByte = (int)b.Memory[b.MemoryPointer];
            memoryByte -= GetSummationValue(memoryByte, byte.MinValue, byte.MaxValue);

            b.Memory[b.MemoryPointer] = (byte)memoryByte;
        });
    }

    private static void RegisterMoveMemoryPointer(IVirtualMachine vm)
    {
        vm.RegisterCommand('>', b =>
        {
            b.MemoryPointer += 
                GetSummationValue(b.MemoryPointer, b.Memory.Length - 1, b.Memory.Length - 1);
        });
		
        vm.RegisterCommand('<', b =>
        {
            b.MemoryPointer -= 
                GetSummationValue(b.MemoryPointer, 0, b.Memory.Length - 1);
        });
    }
	
    private static void RegisterChars(IVirtualMachine vm)
    {
        foreach (var symbol in GetCharArray())
            vm.RegisterCommand(symbol, b => { b.Memory[b.MemoryPointer] = Convert.ToByte(symbol); });
    }
	
    private static int GetSummationValue(int currentValue, int boundToCheck, int maxValue) => 
        currentValue == boundToCheck ? -maxValue : 1;

    private static List<char> GetCharArray()
    {
        var regex = new Regex("[0-9a-zA-Z]");
        var chars = new List<char>();

        for (var i = '0'; i <= 'z'; i++)
            if (regex.IsMatch(i.ToString()))
                chars.Add(i);

        return chars;
    }
}