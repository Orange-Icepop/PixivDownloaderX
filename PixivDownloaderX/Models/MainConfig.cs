namespace PixivDownloaderX.Models;

public class MainConfig
{
    public bool UseProxy { get; set; } = false;
    public string ProxyAddress { get; set; } = "127.0.0.1";
    public int ProxyPort { get; set; } = 7890;
    public bool ConcurrencyLimit { get; set; } = true;
    public string DownloadDirectory { get; set; } = "./Artworks";
}