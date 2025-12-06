using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Chip_8.Interfaces;
using Chip_8.config;
using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chip_8.ViewModels;

partial class SettingsViewModel : ViewModelBase, IConfirmDialogContent
{
    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, Chip8Versions>> _versions = new()
    {
        new("Chip 8 (Legacy)", Chip8Versions.Legacy),
        new("Super-Chip", Chip8Versions.SuperChip),
        new("XO-Chip", Chip8Versions.XoChip)
    };
    
    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, KeyPadLayouts>> _layouts = new()
    {
        new("COSMAC VIP", KeyPadLayouts.CosmacVip),
        new("Qwerty", KeyPadLayouts.Qwerty),
        new("Qwertz", KeyPadLayouts.Qwertz),
    };
    
    [ObservableProperty] private Chip8Versions _selectedVersion = Chip8Versions.Legacy;
    
    [ObservableProperty] private KeyPadLayouts _selectedLayout = KeyPadLayouts.CosmacVip;

    [ObservableProperty] private string _path = string.Empty;
    
    private readonly DialogFactory _dialogFactory;

    public SettingsViewModel(DialogFactory dialogFactory, string? path = null)
    {
        if (path is not null)
            Path = path;
        
        _dialogFactory = dialogFactory;
    }

    [RelayCommand]
    private async Task OpenFilePicker()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            return;
        
        var file = await _dialogFactory.CreateFilePickerDialog(provider);

        if (file.Count > 0) 
            Path = file.First().TryGetLocalPath()!;
    }

    public object? OnConfirm()
    {
        if (string.IsNullOrEmpty(Path)) return null;
        return new InterpreterOptions(SelectedVersion, SelectedLayout, Path);
    }

    public object? OnCancel() => null;

    public string GetConfirmText() => "Run";

    public string GetCancelText() => "Cancel";
}

public record InterpreterOptions(Chip8Versions Version, KeyPadLayouts Layout, string Path);