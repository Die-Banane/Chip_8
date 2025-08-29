using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace Chip_8.Services;

internal class DialogService : IDialogService
{
    public async Task<string?> ShowFileDialog(FilePickerOpenOptions? options = null)
    {
        if (options is null)
        {
            options = new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("binary")
                    {
                        Patterns = new[] { "*.bin", "*.ch8" }
                    }
                }
            };
        }
        
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            return null;

        var files = await provider.OpenFilePickerAsync(options);

        return files?.FirstOrDefault()?.Path.LocalPath;
    }
}
