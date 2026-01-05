using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PixivDownloaderX.Models;
using PixivDownloaderX.Utilities;

namespace PixivDownloaderX.Services;

public class WebService
{
    public WebService(MainConfig mainConfig)
    {
        _mainConfig = mainConfig;
        var handler = new HttpClientHandler()
        {
            MaxConnectionsPerServer = (int)mainConfig.ConcurrencyLimit
        };
        if (mainConfig.UseProxy)
        {
            handler.UseProxy = true;
            if (string.IsNullOrEmpty(mainConfig.ProxyUsername))
            {
                handler.Proxy = new WebProxy("http://" + mainConfig.HttpProxyAddress + ':' + mainConfig.HttpProxyPort,
                    false);
            }
            else
            {
                handler.Proxy = new WebProxy("http://" + mainConfig.HttpProxyAddress + ':' + mainConfig.HttpProxyPort)
                {
                    Credentials = new NetworkCredential(mainConfig.ProxyUsername, mainConfig.ProxyPassword)
                };
            }
        }

        _httpClient = new HttpClient(handler);
    }

    private readonly MainConfig _mainConfig;
    
    public EventHandler<string>? WebEvent;

    private readonly HttpClient _httpClient;

    public async Task Download(DownloadTaskArgs args)
    {
        var dir = Path.Combine(_mainConfig.DownloadDirectory, args.ArtworkId.ToString());
        Directory.CreateDirectory(dir);
        List<DownloadTaskInfo> tasks = [];
        try
        {
            if (args.IsMultiPictures)
            {
                var splits = Converters.ConvertMultiPattern(args.Pattern);
                if (splits.IdFirst)
                {
                    for (var i = args.StartRange; i <= args.EndRange; i++)
                    {
                        tasks.Add(new DownloadTaskInfo(
                            splits.Parts[0] + args.ArtworkId + splits.Parts[1] + i + splits.Parts[2],
                            args.ArtworkId + '-' + i + ".jpg"));
                    }
                }
                else
                {
                    for (var i = args.StartRange; i <= args.EndRange; i++)
                    {
                        tasks.Add(new DownloadTaskInfo(
                            splits.Parts[0] + i + splits.Parts[1] + args.ArtworkId + splits.Parts[2],
                            args.ArtworkId + '-' + i + ".jpg"));
                    }
                }
            }
            else
            {
                var splits = Converters.ConvertSinglePattern(args.Pattern);
                tasks.Add(new DownloadTaskInfo(splits[0] + args.ArtworkId + splits[1], args.ArtworkId + ".jpg"));
            }
        }
        catch (Exception e)
        {
            WebEvent?.Invoke(this, e.Message);
            return;
        }

        await Connect(tasks);
    }

    private async Task Connect(List<DownloadTaskInfo> tasks)
    {
        List<Task> connections = [];
        connections.AddRange(tasks.Select(ConnectSingle));
        await Task.WhenAll(connections);
    }

    private async Task ConnectSingle(DownloadTaskInfo task)
    {
        try
        {
            if (File.Exists(task.Path))
            {
                WebEvent?.Invoke(this, $"文件{task.Path}已存在，跳过下载");
                return;
            }

            WebEvent?.Invoke(this, $"开始下载{task.Url}");
            var response = await _httpClient.GetAsync(task.Url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            await using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                         fileStream = new FileStream(task.Path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await contentStream.CopyToAsync(fileStream);
            }

            WebEvent?.Invoke(this, $"文件已保存到{task.Path}");

        }
        catch (Exception e)
        {
            WebEvent?.Invoke(this, $"下载{task.Url}时发生错误：{e.Message}");
        }
    }
}