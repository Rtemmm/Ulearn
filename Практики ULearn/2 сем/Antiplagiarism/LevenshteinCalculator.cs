using System;
using System.Collections.Generic;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
	public static IEnumerable<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
	{
		var comparisonResults = new List<ComparisonResult>();
		
		for (var i = 0; i < documents.Count; i++)
		for (var j = i + 1; j < documents.Count; j++)
			comparisonResults.Add(LevenshteinDistance(documents[i], documents[j]));
		
		return comparisonResults;
	}

	private static ComparisonResult LevenshteinDistance(DocumentTokens first, DocumentTokens second)
	{
		var opt = new double[first.Count + 1, second.Count + 1];
		
		for (var i = 0; i <= first.Count; ++i) 
			opt[i, 0] = i;
		for (var i = 0; i <= second.Count; ++i) 
			opt[0, i] = i;
		
		for (var i = 1; i <= first.Count; ++i)
		for (var j = 1; j <= second.Count; ++j)
		{
			if (first[i - 1] == second[j - 1])
				opt[i, j] = opt[i - 1, j - 1];
			else
			{
				var distance = Math.Min(TokenDistanceCalculator
					.GetTokenDistance(first[i - 1], second[j - 1]) + opt[i - 1, j - 1], 1 + opt[i, j - 1]);
				
				opt[i, j] = Math.Min(opt[i - 1, j] + 1, distance);
			}
		}

		return new ComparisonResult(first, second, opt[first.Count, second.Count]);
	}
}

