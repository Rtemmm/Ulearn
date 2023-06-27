using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase
            (Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var fullPhrase = new StringBuilder().Append(phraseBeginning);

            for (int i = 0; i < wordsCount; i++)
            {
                var keys = CalculateKeys(fullPhrase);
                var lastTwoWordsKey = keys.Item1;
                var lastWordKey = keys.Item2;

                if (nextWords.ContainsKey(lastTwoWordsKey))
                {
                    fullPhrase.Append(" ");
                    fullPhrase.Append(nextWords[lastTwoWordsKey]);
                }

                else if (nextWords.ContainsKey(lastWordKey))
                {
                    fullPhrase.Append(" ");
                    fullPhrase.Append(nextWords[lastWordKey]);
                }

                else
                    return fullPhrase.ToString();
            }
            return fullPhrase.ToString();
        }

        public static (string, string) CalculateKeys
            (StringBuilder fullPhrase)
        {
            var fullPhraseWords = fullPhrase.ToString().Split();
            var lastTwoWordsKey = string.Empty;

            if (fullPhraseWords.Length >= 2)
                lastTwoWordsKey = fullPhraseWords[fullPhraseWords.Length - 2] + " "
                                       + fullPhraseWords[fullPhraseWords.Length - 1];//тут var не работает

            var lastWordKey = fullPhraseWords[fullPhraseWords.Length - 1].ToString();

            return (lastTwoWordsKey, lastWordKey);
        }
    }
}