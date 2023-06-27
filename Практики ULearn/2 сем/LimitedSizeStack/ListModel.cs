using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public interface ICommand<TItem>
{
    public void Execute(List<TItem> items);

    public void Undo(List<TItem> items);
}

public class CommandAdd<TItem> : ICommand<TItem> 
{
    public TItem Element { get; set; }
    
    public void Execute(List<TItem> items) => items.Add(Element);

    public void Undo(List<TItem> items) => items.RemoveAt(items.IndexOf(Element));
    
}

public class CommandRemove<TItem> : ICommand<TItem>
{
    public TItem Element { get; set; }
    
    public int Index { get; set; }

    public void Execute(List<TItem> items) => items.RemoveAt(Index);

    public void Undo(List<TItem> items) => items.Insert(Index, Element);
}

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    public int UndoLimit { get; }
    private LimitedSizeStack<ICommand<TItem>> Stack { get; }

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        UndoLimit = undoLimit;
        Stack = new LimitedSizeStack<ICommand<TItem>>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        var operation = new CommandAdd<TItem>() { Element = item };
        
        operation.Execute(Items);
        Stack.Push(operation);
    }

    public void RemoveItem(int index)
    {
        var operation = new CommandRemove<TItem>() { Element = Items[index], Index = index };
        
        operation.Execute(Items);
        Stack.Push(operation);
    }

    public bool CanUndo() => Stack.Count > 0;

    public void Undo() => Stack.Pop().Undo(Items);
}