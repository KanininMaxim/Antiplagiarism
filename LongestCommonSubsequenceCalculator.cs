﻿using System;
using System.Collections.Generic;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
    public static List<string> Calculate(List<string> first, List<string> second)
    {
        var opt = CreateOptimizationTable(first, second);
        return RestoreAnswer(opt, first, second);
    }

    private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
    {
        var opt = new int[first.Count + 1, second.Count + 1];

        for (var i = 1; i <= first.Count; i++)
            for (var j = 1; j <= second.Count; j++)
                if (first[i - 1] == second[j - 1])
                    opt[i, j] = opt[i - 1, j - 1] + 1;
                else
                    opt[i, j] = Math.Max(opt[i, j - 1], opt[i - 1, j]);

        return opt;
    }

    private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
    {
        var answer = new List<string>(opt[first.Count, second.Count]);
        var counter = opt[first.Count, second.Count];

        for (var i = first.Count - 1; i >= 0 && counter > 0; i--)
            for (var j = second.Count - 1; j >= 0 && counter > 0; j--)
                if (first[i] == second[j])
                {
                    answer.Add(first[i]);
                    i--;
                    counter--;
                }
                else if (opt[i, j + 1] == opt[i + 1, j + 1] && i > 0)
                {
                    i--;
                    j++;
                }

        answer.Reverse();
        return answer;
    }
}