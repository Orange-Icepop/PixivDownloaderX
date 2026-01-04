namespace PixivDownloaderX.Utilities;

public static class Validators
{
    public static string? ValidateSinglePattern(string pattern)
    {
        var res = pattern.Split("{id}");
        return res.Length switch
        {
            2 => null,
            < 2 => "该单图表达式不包含{id}",
            _ => "单图表达式不得包含多个{id}"
        };
    }

    public static string? ValidateMultiPattern(string pattern)
    {
        var splits1 = pattern.Split("{id}");
        switch (splits1.Length)
        {
            case < 2: return "该多图表达式不包含{id}";
            case > 2: return "多图表达式不得包含多于一个{id}";
        }

        var splits2 = splits1[0].Split("{idx}");
        switch (splits2.Length)
        {
            case > 2: return "多图表达式不得包含多于一个{idx}";
            case 2: return null;
        }

        splits2 = splits1[1].Split("{idx}");
        return splits2.Length switch
        {
            > 2 => "多图表达式不得包含多于一个{idx}",
            < 2 => "该多图表达式不包含{idx}",
            _ => null
        };
    }
}