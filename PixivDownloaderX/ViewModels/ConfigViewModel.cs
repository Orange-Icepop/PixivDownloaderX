using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PixivDownloaderX.Models;
using ReactiveUI.SourceGenerators;

namespace PixivDownloaderX.ViewModels;

public class ConfigViewModel
{
    private const string ConfigFileName = "Config.json";

    private const string PatternConfigFileName = "PatternConfig.json";

    private static readonly string s_baseDir = AppDomain.CurrentDomain.BaseDirectory;

    private static readonly string s_configFilePath = Path.Combine(s_baseDir, ConfigFileName);
    private static readonly string s_patternConfigFilePath = Path.Combine(s_baseDir, PatternConfigFileName);

    private static readonly JsonSerializerOptions s_jsonSerializerOptions =
        new() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public MainConfig CurrentMainConfig { get; private set; } = new();

    public static Dictionary<uint, PatternConfig> DefaultPatternConfig { get; private set; } =
        new()
        {
            { 0, new PatternConfig("pixiv.nl", "https://pixiv.cat/{id}.jpg", "https://pixiv.cat/{id}-{idx}.jpg") }
        };

    [Reactive] public Dictionary<uint, PatternConfig> CurrentPatternConfig { get; private set; }

    private EventHandler<string>? _configChanged;

    public ConfigViewModel(EventHandler<string>? configChanged = null)
    {
        _configChanged = configChanged;
        Init();
        if (!ReadMainConfig()) _configChanged?.Invoke(this, "由于格式错误等原因，无法读取主配置文件，改用默认配置文件。");
        if (!ReadPatternConfig()) _configChanged?.Invoke(this, "由于格式错误等原因，无法读取模板配置文件，改用默认配置文件。");
    }

    private static void Init()
    {
        if (!File.Exists(s_configFilePath))
            File.WriteAllText(s_configFilePath, JsonSerializer.Serialize(new MainConfig(), s_jsonSerializerOptions));
        if (!File.Exists(s_patternConfigFilePath))
            File.WriteAllText(s_patternConfigFilePath,
                JsonSerializer.Serialize(DefaultPatternConfig, s_jsonSerializerOptions));
    }

    private bool ReadMainConfig()
    {
        try
        {
            if (!File.Exists(s_configFilePath)) return false;
            var config = JsonSerializer.Deserialize<MainConfig>(File.ReadAllText(s_configFilePath), s_jsonSerializerOptions);
            if (config is null)
            {
                CurrentMainConfig = new MainConfig();
                return false;
            }
            CurrentMainConfig = config;
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    private bool ReadPatternConfig()
    {
        try
        {
            if (!File.Exists(s_patternConfigFilePath)) return false;
            var config =
                JsonSerializer.Deserialize<Dictionary<uint, PatternConfig>>(File.ReadAllText(s_patternConfigFilePath), s_jsonSerializerOptions);
            if (config is null)
            {
                CurrentPatternConfig = DefaultPatternConfig;
                return false;
            }
            CurrentPatternConfig = config;
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}