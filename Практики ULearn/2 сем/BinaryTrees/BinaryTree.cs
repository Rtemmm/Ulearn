using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    private TreeNode<T> _treeNode;
    
    public T this[int index]
    {
        get
        {
            if (_treeNode == null || index < 0 || index >= _treeNode.Size) throw new IndexOutOfRangeException();
            
            var currentNode = _treeNode;
            
            while (true)
            {
                var leftSize = currentNode.Left?.Size ?? 0;
                
                if (index == leftSize)
                    return currentNode.Value;
                
                if (index < leftSize)
                    currentNode = currentNode.Left;
                else
                {
                    currentNode = currentNode.Right;
                    index -= 1 + leftSize;
                }
            }
        }  
    }
    
    public void Add(T value)
    {
        if (_treeNode == null) 
            _treeNode = new TreeNode<T>(value);
        else
        {
            AddLoop(value);
        }
    }

    private void AddLoop(T value)
    {
        var currentNode = _treeNode;
        while (true)
        {
            if (currentNode.Value.CompareTo(value) > 0)
            {
                if (currentNode.Left == null)
                {
                    currentNode.Left = new TreeNode<T>(value);
                    break;
                }
                currentNode = currentNode.Left;
            }
            else
            {
                if (currentNode.Right == null)
                {
                    currentNode.Right = new TreeNode<T>(value);
                    break;
                }
                currentNode = currentNode.Right;
            }
        }
    }
    
    public bool Contains(T value)
    {
        if (Equals(_treeNode, null))
            return false;

        var currentTreeNode = _treeNode;
        
        while (!Equals(currentTreeNode, null))
        {
            if (Equals(currentTreeNode.Value, value))
                return true;
            
            currentTreeNode = currentTreeNode.Value.CompareTo(value) < 0 
                ? currentTreeNode.Right
                : currentTreeNode.Left;
        }

        return false;
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        return (_treeNode == null 
            ? Enumerable.Empty<T>().GetEnumerator()
            : _treeNode?.GetValues().GetEnumerator())!;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class TreeNode<T> where T : IComparable
{
    public T Value { get; }
    private TreeNode<T> _parent;
    private TreeNode<T> _left;
    private TreeNode<T> _right;
    
    public TreeNode<T> Left
    {
        get => _left;
        set
        {
            if (_left != null)
                OnChildCountChanged(-_left.Size); 
            
            _left = value;
            
            if (value == null) 
                return;
            
            OnChildCountChanged(value.Size);
            value._parent = this;
        }
    }
    
    public TreeNode<T> Right
    {
        get => _right;
        set
        {
            if (_right != null)
                OnChildCountChanged(-_right.Size);
            
            _right = value;
            
            if (value == null)
                return;
            
            OnChildCountChanged(value.Size);
            value._parent = this;
        }
    }
    
    public int Size { get; private set; }

    public TreeNode(T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        
        Value = value;
        Size = 1;
    }

    public IEnumerable<T> GetValues()
    {
        if (Left != null)
            foreach (var value in Left.GetValues())
                yield return value;
        
        yield return Value;

        if (Right == null) yield break;
        {
            foreach (var value in Right.GetValues())
                yield return value;
        }
    }
    
    private void OnChildCountChanged(int delta)
    {
        Size += delta;
        _parent?.OnChildCountChanged(delta);
    }
}
