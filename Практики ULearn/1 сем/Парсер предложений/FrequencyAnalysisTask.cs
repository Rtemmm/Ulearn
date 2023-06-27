using System.Collections.Generic;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static void IncrementNGramForKeys
            (Dictionary<string, Dictionary<string, int>> nGram,
            string key, string wordKey)
        {
            if (!nGram.ContainsKey(key))
                nGram.Add(key, new Dictionary<string, int>());
            if (!nGram[key].ContainsKey(wordKey))
                nGram[key].Add(wordKey, 1);
            else
                nGram[key][wordKey]++;
        }

        public static Dictionary<string, Dictionary<string, int>> GetNGram(List<List<string>> text, int n)
        {
            var nGram = new Dictionary<string, Dictionary<string, int>>();
            var key = string.Empty;
            var wordKey = string.Empty;

            foreach (var sentence in text)
                for (var i = 0; i < sentence.Count - (n - 1); i++)
                {
                    if (n == 2)
                    {
                        key = sentence[i];
                        wordKey = sentence[i + 1];
                    }
                    else
                    {
                        key = sentence[i] + " " + sentence[i + 1];
                        wordKey = sentence[i + 2];
                    }

                    IncrementNGramForKeys(nGram, key, wordKey);
                }

            return nGram;
        }

        public static void CalculateMostFrequentWords
            (Dictionary<string, Dictionary<string, int>> nGram, 
            Dictionary<string, string> result)
        {
            foreach (var firstKey in nGram)
            {
                var maxValue = 0;
                var frequentWord = string.Empty;

                foreach (var secondKey in firstKey.Value)
                {
                    if (secondKey.Value == maxValue & string.CompareOrdinal(frequentWord, secondKey.Key) > 0)
                        frequentWord = secondKey.Key;

                    if (secondKey.Value > maxValue)
                    {
                        frequentWord = secondKey.Key;
                        maxValue = secondKey.Value;
                    }
                }
                result.Add(firstKey.Key, frequentWord);
            }
        }

        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            
            var bigrams = GetNGram(text, 2);
            var trigrams = GetNGram(text, 3);
            

            CalculateMostFrequentWords(bigrams, result);
            CalculateMostFrequentWords(trigrams, result);

            return result;
        }
    }
}