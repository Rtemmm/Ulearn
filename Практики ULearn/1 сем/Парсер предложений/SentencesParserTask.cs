using System;
using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static string[] SentenceSeparators = new string[] { ".", "!", "?", ";", ":", "(", ")" };

        public static List<string> GetWordsList(string sentence)
        {
            var wordsBuilder = new StringBuilder ();
            var wordsList = new List<string> ();

            for (int i = 0; i < sentence.Length; i++)
            {
                if (char.IsLetter(sentence[i]) || sentence[i].Equals('\''))
                    wordsBuilder.Append(sentence[i]);
                else
                {
                    wordsList.Add(wordsBuilder.ToString());
                    wordsBuilder.Clear();
                }
                    
            }

            wordsList.Add(wordsBuilder.ToString());

            return wordsList;
        }

        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>(); 
            var sentences = text.ToLower().Split(SentenceSeparators, StringSplitOptions.RemoveEmptyEntries);

            foreach (var sentence in sentences)
            {
                var wordsList = GetWordsList(sentence);

                while (wordsList.Contains(""))
                    wordsList.RemoveAt(wordsList.IndexOf(""));

                if (wordsList.Count == 0)
                    continue;

                sentencesList.Add(wordsList);
            }
            return sentencesList;
        }
    }
}