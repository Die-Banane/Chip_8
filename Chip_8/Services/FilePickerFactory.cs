using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace Chip_8.Services;

public class FilePickerFactory
{
    public async Task<IReadOnlyList<IStorageFile>> Create(IStorageProvider provider)
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