namespace PixivDownloaderX.Models;

public class DownloadTaskArgs
{
    public string Pattern { get; set; } = string.Empty;
    public uint ArtworkId { get; set; } = 0;
    public bool IsMultiPictures { get; set; } = false;
    public uint StartRange { get; set; } = 0;
    public uint EndRange { get; set; } = 0;
}