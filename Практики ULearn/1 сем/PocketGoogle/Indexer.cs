using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private readonly Dictionary<string, Dictionary<int, List<int>>> idWordInDocument = 
            new Dictionary<string, Dictionary<int, List<int>>>();

        public void Add(int id, string documentText)
        {
            var words = documentText.Split(new char[] { ' ', '.', ',', '!', '?', ':', '-', '-', '\r', '\n' });

            var govno = 0;

            foreach (var word in words)
            {
                if (!idWordInDocument.ContainsKey(word))
                    idWordInDocument.Add(word, new Dictionary<int, List<int>>());

                if (!idWordInDocument[word].ContainsKey(id))
                    idWordInDocument[word].Add(id, new List<int>());
               
                idWordInDocument[word][id].Add(govno);

                govno += word.Length + 1;     
            }
        }

        public List<int> GetIds(string word)
        {
            var ids = new List<int>();

            if (idWordInDocument.ContainsKey(word))
                foreach (var id in idWordInDocument[word].Keys)
                    ids.Add(id);
            
            return ids;
        }

        public List<int> GetPositions(int id, string word)
        {
            if (idWordInDocument.ContainsKey(word) && idWordInDocument[word].ContainsKey(id))
                return idWordInDocument[word][id];

            return new List<int>();
        } 
        
        public void Remove(int id)
        {
            foreach (var word in idWordInDocument.Keys)
                idWordInDocument[word].Remove(id);
        }
    }
}
