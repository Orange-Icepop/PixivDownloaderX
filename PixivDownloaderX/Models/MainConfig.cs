namespace PixivDownloaderX.Models;

public class MainConfig
{
    public bool UseProxy { get; set; } = false;
    public string HttpProxyAddress { get; set; } = "127.0.0.1";
    public uint HttpProxyPort { get; set; } = 7890;
    public string ProxyUsername { get; set; } = string.Empty;
    public string ProxyPassword { get; set; } = string.Empty;
    public uint ConcurrencyLimit { get; set; } = 3;
    public string DownloadDirectory { get; set; } = "./Artworks";
}