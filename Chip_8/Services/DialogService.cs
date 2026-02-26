using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Chip_8.Interfaces;
using Chip_8.Views;

namespace Chip_8.Services;

public class DialogService : IDialogService
{
    public async Task<TResult> ShowDialog<TResult>(IDialog<TResult> content)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            throw new InvalidOperationException("The Application needs to have a MainWindow");

        var dialog = new DialogWindow
        {
            Content = content
        };
        
        await dialog.ShowDialog(desktop.MainWindow!);

        return content.GetResult();
    }
}