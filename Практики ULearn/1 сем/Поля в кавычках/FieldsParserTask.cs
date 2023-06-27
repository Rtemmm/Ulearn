using System.Collections.Generic;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("hello  world", new[] { "hello", "world" })]
        [TestCase(" hello world ", new[] { "hello", "world" })]
        [TestCase("' '", new[] { " " })]
        [TestCase("''", new[] { "" })]
        [TestCase("", new string[0])]
        [TestCase("'ddddd ", new[] { "ddddd " })]
        [TestCase(" 'hello world' ", new[] { "hello world" })]
        [TestCase(@"'""dd""", new[] { @"""dd""" })]
        [TestCase(@"fd""hello er ff qqq""", new[] { "fd", "hello er ff qqq" })]
        [TestCase(@"""\\""", new[] { "\\" })]
        [TestCase(@"'f\'s\''", new[] { "f's'" })]
        [TestCase(@"""f \""s\""""", new[] { @"f ""s""" })]
        [TestCase(@"""f s t f""f", new[] { "f s t f", "f" })]
        [TestCase(@"""'dd'""", new[] { @"'dd'" })]

        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var tokens = new List<Token>();
            var startIndex = 0;
            Token token;

            while (startIndex < line.Length)
            {
                if (line[startIndex].Equals('\'') || line[startIndex].Equals('\"'))
                    token = ReadQuotedField(line, startIndex);
                else if (char.IsWhiteSpace(line[startIndex]))
                {
                    startIndex++;
                    continue;
                }
                else
                    token = ReadField(line, startIndex);

                tokens.Add(token);
                startIndex = token.GetIndexNextToToken();
            }
            return tokens;
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }

        private static Token ReadField(string line, int startIndex)
        {
            var length = 0;

            for (int i = startIndex; i < line.Length; i++)
            {
                if (line[i].Equals('\'') || line[i].Equals('\"') || char.IsWhiteSpace(line[i]))
                    break;
                length++;
            }

            line = line.Substring(startIndex, length);
            return new Token(line, startIndex, length);
        }   
    }
}