using System;
using System.Collections.Generic;

namespace PixivDownloaderX.Utilities;

public static class Converters
{
    public static List<string> ConvertSinglePattern(string pattern)
    {
        var splits = pattern.Split("{id}");
        return splits.Length switch
        {
            < 2 => throw new ArgumentException("该单图表达式不包含{id}", nameof(pattern)),
            > 2 => throw new ArgumentException("单图表达式不得包含多于一个{id}", nameof(pattern)),
            _ => [..splits]
        };
    }

    public static (List<string> Parts, bool IdFirst) ConvertMultiPattern(string pattern)
    {
        var splits1 = pattern.Split("{id}");
        switch (splits1.Length)
        {
            case < 2:
                throw new ArgumentException("该多图表达式不包含{id}", nameof(pattern));
            case > 2:
                throw new ArgumentException("多图表达式不得包含多于一个{id}", nameof(pattern));
        }

        var splits2 = splits1[0].Split("{idx}");
        switch (splits2.Length)
        {
            case > 2:
                throw new ArgumentException("多图表达式不得包含多于一个{idx}", nameof(pattern));
            case 2:
                return ([splits2[0], splits2[1], splits1[1]], false);
        }

        splits2 = splits1[1].Split("{idx}");
        return splits2.Length switch
        {
            > 2 => throw new ArgumentException("多图表达式不得包含多于一个{idx}", nameof(pattern)),
            < 2 => throw new ArgumentException("该多图表达式不包含{idx}", nameof(pattern)),
            _ => ([splits1[0], splits2[0], splits2[0]], true)
        };
    }
}