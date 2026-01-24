using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Chip_8.Interfaces;
using Chip_8.ViewModels;
using Chip_8.Views;

namespace Chip_8.Services;

public class DialogFactory(ConfirmDialogWindow confirmDialogWindow)
{
    public async Task<object?> CreateConfirmDialog<TDialogContent>(Window? owner = null, params object[]? args)
        where TDialogContent : IConfirmDialogContent
    {
        var dialog = confirmDialogWindow;

        try
        {
            dialog.DataContext = new ConfirmDialogViewModel((TDialogContent)Activator.CreateInstance(typeof(TDialogContent), args)!);
        }
        catch (Exception)
        {
            //TODO: make better error handling
            throw new ArgumentException("Wrong args");
        }

        if (owner is null &&
            Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
                owner = desktop.MainWindow;
        }
        
        //TODO: make Result type for the response to avoid returning null
        if (owner == null) return null;
        
        return await dialog.ShowDialog<object?>(owner);
    }
    
    public async Task<IReadOnlyList<IStorageFile>> CreateFilePickerDialog(IStorageProvider provider)
    {
        var options = new FilePickerOpenOptions
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
        };
        
        return await provider.OpenFilePickerAsync(options);
    }
}