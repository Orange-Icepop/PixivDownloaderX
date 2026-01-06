using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PixivDownloaderX.Models;
using PixivDownloaderX.Services;
using PixivDownloaderX.Utilities;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PixivDownloaderX.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [Reactive] private int _patternIndex;
    [Reactive] [NumericValidator] private string _artworkId = string.Empty;
    [Reactive] private bool _isMultiPictures = false;
    [Reactive] [NumericValidator] private string _startRange = "0";
    [Reactive] [NumericValidator] private string _endRange = "0";
    private ConfigViewModel? _configViewModel;
    private WebService _webService;

    public MainViewModel()
    {
        _configViewModel = new ConfigViewModel();
        _configViewModel.ConfigEvent += (_, s) => SystemMessage.Add(new ApplicationMessage(DateTime.Now, s));
        _configViewModel.Init();
        _webService = new WebService(_configViewModel.CurrentMainConfig);
        _webService.WebEvent += (_, s) => SystemMessage.Add(new ApplicationMessage(DateTime.Now, s));
        _patternConfigListHelper = _configViewModel.WhenAnyValue(c => c.CurrentPatternConfig)
            .Select(o => o.Values)
            .Select(c => new ObservableCollection<PatternConfig>(c))
            .ToProperty(this, nameof(PatternConfigList), scheduler: RxApp.MainThreadScheduler);
    }

    [Reactive] private ObservableCollection<ApplicationMessage> _systemMessage = [];

    [ObservableAsProperty] private ObservableCollection<PatternConfig> _patternConfigList = [];

    [ReactiveCommand]
    private async Task Download()
    {
        if (!DownloadTaskArgs.TryCreate(
                IsMultiPictures
                    ? PatternConfigList[PatternIndex].MultiPattern
                    : PatternConfigList[PatternIndex].SinglePattern,
                ArtworkId, IsMultiPictures, StartRange, EndRange, out var args))
        {
            SystemMessage.Add(new ApplicationMessage(DateTime.Now, "下载信息有误，请检查"));
            return;
        }

        await _webService.Download(args);
    }
}