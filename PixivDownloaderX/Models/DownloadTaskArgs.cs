namespace PixivDownloaderX.Models;

public class DownloadTaskArgs
{
    public required string Pattern { get; set; }
    public required uint ArtworkId { get; set; }
    public required bool IsMultiPictures { get; set; }
    public required uint StartRange { get; set; }
    public required uint EndRange { get; set; }
}