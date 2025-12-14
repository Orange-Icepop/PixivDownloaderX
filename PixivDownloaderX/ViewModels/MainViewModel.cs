using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using PixivDownloaderX.Models;
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
    public MainViewModel()
    {
        _configViewModel = new ConfigViewModel(((_, s) => SystemMessage.Add(new ApplicationMessage(DateTime.Now, s))));
        _configViewModel.WhenAnyValue(c=>c.CurrentPatternConfig)
            .Select(o=>o.Values)
            .Select(c=>new ObservableCollection<PatternConfig>(c))
            .ToProperty(this, v => v.PatternConfigList);
    }

    [Reactive] public ObservableCollection<ApplicationMessage> SystemMessage { get; } = [];

    [ObservableAsProperty] private ObservableCollection<PatternConfig> _patternConfigList = [];
}