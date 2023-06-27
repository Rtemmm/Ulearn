using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree;
public class DiskTreeTask
{
    public class Root
    {
        private readonly string _name;
        private readonly Dictionary<string, Root> _nodes = new();

        public Root(string name)
        {
            _name = name;
        }

        public Root GetDirection(string subRoot)
        {
            return _nodes.TryGetValue(subRoot, out var node) 
                ? node : _nodes[subRoot] = new Root(subRoot);
        }

        public List<string> MakeConcluson(int i, List<string> list)
        {
            if (i >= 0)
                list.Add(new string(' ', i) + _name);
            i++;

            return _nodes.Values
                .OrderBy(root => root._name, StringComparer.Ordinal)
                .Aggregate(list, (current, child) => child.MakeConcluson(i, current));
        }
    }

    public static IEnumerable<string> Solve(List<string> input)
    {
        var root = new Root("");
        
        foreach (var node in input.Select(name => name.Split('\\'))
                     .Select(path => path
                         .Aggregate(root, (current, item) => current.GetDirection(item)))) { }

        return root.MakeConcluson(-1, new List<string>());
    }
}
