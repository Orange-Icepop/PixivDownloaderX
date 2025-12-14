namespace PixivDownloaderX.Models;

public class PatternConfig(string name, string singlePattern, string multiPattern)
{
    public string Name { get; } = name;
    public string SinglePattern { get; } = singlePattern;
    public string MultiPattern { get; } = multiPattern;
}