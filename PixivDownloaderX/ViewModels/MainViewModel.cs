using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using PixivDownloaderX.Models;
using PixivDownloaderX.Services;
using PixivDownloaderX.Utilities;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace PixivDownloaderX.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    [Reactive] public int PatternIndex { get; set; }
    [Reactive] [ArtworkIdValidator] public string ArtworkId { get; set; } = string.Empty;
    [Reactive] public bool IsMultiPictures { get; set; } = false;
    [Reactive] public string StartRange { get; set; } = string.Empty;
    [Reactive] public string EndRange { get; set; } = string.Empty;
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

    [Reactive] public ObservableCollection<ApplicationMessage> SystemMessage { get; set; } = [];

    [ObservableAsProperty] private ObservableCollection<PatternConfig> _patternConfigList = [];
}