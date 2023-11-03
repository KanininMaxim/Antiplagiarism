using System;
using System.Collections.Generic;
using System.Linq;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
    {
        var documentsDistance = new List<ComparisonResult>();

        for (int i = 0; i < documents.Count; i ++)
            for (int j = i + 1; j < documents.Count; j++)
            {
                var document1 = documents[i];
                var document2 = documents[j];
                var opt = new double[document1.Count + 1, document2.Count + 1];
                for (var k = 0; k <= document1.Count; ++k) opt[k, 0] = k;
                for (var k = 0; k <= document2.Count; ++k) opt[0, k] = k;
                for (var k = 1; k <= document1.Count; ++k)
                    for (var l = 1; l <= documents[j].Count; ++l)
                        FillingOutTheTableLevenstein(document1, document2, k, l, opt);
                documentsDistance.Add(new ComparisonResult(document1, document2, opt[document1.Count, document2.Count]));
            }
        return documentsDistance;
    }

    public void FillingOutTheTableLevenstein(DocumentTokens document1, DocumentTokens document2, int k, int l, double[,] opt)
    {
        if (document1[k - 1] == document2[l - 1])
            opt[k, l] = opt[k - 1, l - 1];
        else
            opt[k, l] = Math.Min(Math.Min(opt[k, l - 1] + 1, opt[k - 1, l] + 1), opt[k - 1, l - 1]
                + TokenDistanceCalculator.GetTokenDistance(document1[k - 1], document2[l - 1]));
    }
}