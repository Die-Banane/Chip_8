using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chip_8.Services;

internal interface IDialogService
{
    public Task<string?> ShowFileDialog(FilePickerOpenOptions? options = null);
}