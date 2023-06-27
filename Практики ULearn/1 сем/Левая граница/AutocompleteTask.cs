using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];
            
            return string.Empty;
        }

        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var countByPrefix = GetCountByPrefix(phrases, prefix);

            if (countByPrefix <= 0)
                return new string[0];

            if (countByPrefix < count)
                count = countByPrefix;

            var top = new string[count];

            for (int i = 0; i < count; i++)
                top[i] = phrases[leftBorder + i];

            return top;
        }

        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            var rightBorder = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);

            return rightBorder - leftBorder - 1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [TestCase(new string[0], "a", "")]
        [TestCase(new string[] { "first", "second", "third" }, "", "first")]
        [TestCase(new string[] { "first", "second", "third" }, "g", "")]
        [TestCase(new string[] { "a", "aa", "aaa" }, "a", "a")]
        public void TestFindFirstByPrefix(string[] phrases, string prefix, string expectedResult)
        {
            var actualResult = AutocompleteTask.FindFirstByPrefix(phrases, prefix);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(new string[0], "a", 10, new string[0])]
        [TestCase(new string[] { "a", "aa", "aaa", "aaaa", "b", "c", "d", "e" },
            "", 5, new string[5] { "a", "aa", "aaa", "aaaa", "b" })]
        [TestCase(new string[] { "adin", "aerrr", "dds", "frr" }, "g", 10, new string[0])]
        [TestCase(new string[] { "adin", "aerrr", "dds", "frr" }, "a", 10, new string[] { "adin", "aerrr" })]
        [TestCase(new string[] { "a", "aa", "aaa", "aaaa", "b", "c", "d", "e" },
            "a", 10, new string[] { "a", "aa", "aaa", "aaaa" })]
        public void TestGetTopByPrefix(string[] phrases, string prefix, int count, string[] expectedResult)
        {
            var actualResult = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(new string[0], "a", 0)]
        [TestCase(new string[] { "mama", "raz", "dva", "tri" }, "", 4)]
        [TestCase(new string[] { "adin", "aerrr", "dds", "frr" }, "g", 0)]
        [TestCase(new string[] { "adin", "aerrr", "dds", "frr" }, "a", 2)]
        [TestCase(new string[] { "a", "aa", "aaa", "aaaa", "b", "c", "d", "e" }, "a", 4)]
        public void TestGetCountByPrefix(string[] phrases, string prefix, int expectedResult)
        {
            var actualResult = AutocompleteTask.GetCountByPrefix(phrases, prefix);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}