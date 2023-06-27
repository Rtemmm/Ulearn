using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'novie'", 0, "novie", 7)]
        [TestCase("\"testi\"", 0, "testi", 7)]
        [TestCase("bolshe \"ne'\"", 7, "ne'", 5)]
        [TestCase(@"'spisival\' swear'", 0, "spisival' swear", 18)]
        [TestCase("'", 0, "", 1)]
        [TestCase("\"", 0, "", 1)]
        [TestCase(@"'alo\' alo\'", 0, "alo' alo'", 12)]
        [TestCase(@"'kod\' sam'pisal", 0, "kod' sam", 11)]
        [TestCase(@"'\'\''xyz", 0, "''", 6)]
        [TestCase("'ff\"\"'", 0, "ff\"\"", 6)]
        [TestCase("\"qwe\"sheet", 0, "qwe", 5)]
        [TestCase("hello\"how'\"", 5, "how'", 6)]
        [TestCase("'about\"\"'", 0, "about\"\"", 9)]
        [TestCase("'zero' ballov'", 0, "zero", 6)]
        [TestCase("'today?", 0, "today?", 7)]
        [TestCase("\"\\\\\"", 0, "\"", 4)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var field = new StringBuilder();
            var startQuote = line[startIndex];
            var value = "";
            var length = 0;

            for (var i = startIndex + 1; i < line.Length; i++)
            {
                if (line[i].Equals(startQuote) && !line[i - 1].Equals('\\'))
                {
                    value = field.ToString();
                    length = line.Substring(startIndex, (i - startIndex) + 1).Length;

                    return new Token(value, startIndex, length);
                }
                if (!line[i].Equals('\\'))
                    field.Append(line[i]);
                else
                    length++;
            }

            value = field.ToString();
            length = line.Length;
            
            return new Token(value, startIndex, length);
        }
    }
}
