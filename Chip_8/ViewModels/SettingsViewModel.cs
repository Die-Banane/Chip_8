using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Chip_8.Data;
using Chip_8.Interfaces;
using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chip_8.ViewModels;

partial class SettingsViewModel : ViewModelBase, IDialog<InterpreterOptions>
{
    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, Chip8Versions>> _versions =
    [
        new("Chip 8 (Legacy)", Chip8Versions.Legacy),
        new("Super-Chip", Chip8Versions.SuperChip),
        new("XO-Chip", Chip8Versions.XoChip)
    ];
    
    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, KeyPadLayouts>> _layouts =
    [
        new("Qwerty", KeyPadLayouts.Qwerty),
        new("Qwertz", KeyPadLayouts.Qwertz),
        new("Azerty", KeyPadLayouts.Azerty)
    ];
    
    //default settings
    [ObservableProperty] private Chip8Versions _selectedVersion = Chip8Versions.Legacy;
    [ObservableProperty] private KeyPadLayouts _selectedLayout = KeyPadLayouts.Qwerty;
    [ObservableProperty] private int _cpuFrequency = 700;
    [ObservableProperty] private string _path = string.Empty;
    
    private readonly FilePickerFactory _pickerFactory;

    public SettingsViewModel(FilePickerFactory pickerFactory, string? path = null)
    {
        if (path is not null)
            Path = path;
        
        _pickerFactory = pickerFactory;
    }

    [RelayCommand]
    private async Task OpenFilePicker()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            return;
        
        var file = await _pickerFactory.Create(provider);

        if (file.Count > 0)
            Path = file.First().TryGetLocalPath()!;
    }

    public InterpreterOptions GetResult()
    {
        if (string.IsNullOrEmpty(Path))
            return InterpreterOptions.InvalidOptions;

        return new InterpreterOptions(SelectedVersion, SelectedLayout, Path, CpuFrequency);
    }
}

public record InterpreterOptions(Chip8Versions Version, KeyPadLayouts Layout, string Path, int Frequency)
{
    public static readonly InterpreterOptions InvalidOptions = 
        new
        (
            default,
            default,
            string.Empty,
            0
        );
}