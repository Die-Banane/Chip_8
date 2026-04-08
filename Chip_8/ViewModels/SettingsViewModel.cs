using System.Collections.Generic;
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
    //default settings
    [ObservableProperty] private int _selectedVersion = (int)Chip8Versions.Legacy;
    [ObservableProperty] private int _selectedLayout = (int)KeyPadLayouts.Qwerty;
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

        return DialogResult<InterpreterOptions>.Ok(new((Chip8Versions)SelectedVersion, (KeyPadLayouts)SelectedLayout, Path, CpuFrequency));
    }
}

public enum Chip8Versions
{
    Legacy,
    SuperChip,
    XoChip
}
    
public enum KeyPadLayouts
{
    Qwerty,
    Qwertz,
    Azerty
}

public record InterpreterOptions(Chip8Versions Version, KeyPadLayouts Layout, string Path, int Frequency);