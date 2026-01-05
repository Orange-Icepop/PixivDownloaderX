namespace PixivDownloaderX.Models;

public class DownloadTaskArgs
{
    public static bool TryCreate(string pattern, string artworkId, bool isMultiPictures, string start, string end,
        out DownloadTaskArgs args)
    {
        args = new DownloadTaskArgs();
        if (!ulong.TryParse(artworkId, out var id)) return false;
        if (isMultiPictures)
        {
            if (!uint.TryParse(start, out var startRange)) return false;
            if (!uint.TryParse(end, out var endRange)) return false;
            if (startRange > endRange) return false;
            args.StartRange = startRange;
            args.EndRange = endRange;
        }

        args.Pattern = pattern;
        args.ArtworkId = id;
        args.IsMultiPictures = isMultiPictures;
        return true;
    }

    private DownloadTaskArgs()
    {
        
    }

    public string Pattern { get; set; } = string.Empty;
    public ulong ArtworkId { get; set; }
    public bool IsMultiPictures { get; set; }
    public uint StartRange { get; set; }
    public uint EndRange { get; set; }
}