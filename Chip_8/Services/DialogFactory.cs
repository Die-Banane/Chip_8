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
using Microsoft.Extensions.DependencyInjection;

namespace Chip_8.Services;

public class DialogFactory(IServiceProvider serviceProvider)
{
    public async Task<object?> CreateConfirmDialog<TDialogContent>(Window? owner = null, params object[]? args)
        where TDialogContent : IConfirmDialogContent
    {
        var dialog = serviceProvider.GetRequiredService<ConfirmDialogWindow>();
        
        dialog.DataContext = new ConfirmDialogViewModel((TDialogContent)Activator.CreateInstance(typeof(TDialogContent), args)!);

        if (owner is null &&
            Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
                owner = desktop.MainWindow;
        }
        
        return await dialog.ShowDialog<object?>(owner!);
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