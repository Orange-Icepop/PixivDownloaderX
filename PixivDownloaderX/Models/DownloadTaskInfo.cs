namespace PixivDownloaderX.Models;

public class DownloadTaskInfo(string url, string path)
{
    public string Url { get; set; } = url;
    public string Path { get; set; } = path;
}