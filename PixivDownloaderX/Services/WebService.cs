using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using PixivDownloaderX.Models;

namespace PixivDownloaderX.Services;

public static class WebService
{
    private static HttpClient _httpClient = new()
    {
        Timeout = new TimeSpan(0, 0, 20),
    };

    public static async Task<string?> Download(DownloadTaskArgs args, MainConfig mainConfig)
    {
        try
        {
            Directory.CreateDirectory(mainConfig.DownloadDirectory);
        }
        catch (Exception e)
        {
            return "无法创建下载目录：" + e.Message;
        }
    }

    private static List<string> ParseArgs(DownloadTaskArgs args)
    {
        if (args.IsMultiPictures)
        {
            var sep = args.Pattern.Split("{id}");
            if (sep.Length != 2) throw new ArgumentException("Pattern doesn't contain exactly 1 time of {id}");
        }
    }
}