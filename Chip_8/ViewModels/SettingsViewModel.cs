using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Chip_8.Data;
using Chip_8.Interfaces;
using Chip_8.Views;
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
    
    private readonly IStorageProvider _storageProvider;

    public SettingsViewModel(MainWindow mainWindow, string? path = null)
    {
        if (path is not null)
            Path = path;
        
        _storageProvider = mainWindow.StorageProvider;
    }

    [RelayCommand]
    private async Task OpenFilePicker()
    {
        var file = await _storageProvider.OpenFilePickerAsync
        (
            new FilePickerOpenOptions
            {
                Title = "pick program to run",
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    new("Roms")
                    {
                        Patterns = ["*.ch8", "*.bin"]
                    }
                }
            }
        );

        if (file.Count > 0)
            Path = file.First().TryGetLocalPath()!;
    }
    
    public void Cancel() => Path = string.Empty;
    
    public DialogResult<InterpreterOptions> GetResult()
    {
        if (string.IsNullOrEmpty(Path))
            return DialogResult<InterpreterOptions>.Cancelled();

        return DialogResult<InterpreterOptions>.Ok(new (SelectedVersion, SelectedLayout, Path, CpuFrequency));
    }
}

public record InterpreterOptions(Chip8Versions Version, KeyPadLayouts Layout, string Path, int Frequency);