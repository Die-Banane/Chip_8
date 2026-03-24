using System.Threading.Tasks;
using Chip_8.Data;
using Chip_8.Interfaces;
using Chip_8.Views;

namespace Chip_8.Services;

public class DialogService(MainWindow mainWindow) : IDialogService
{
    public async Task<DialogResult<TResult>> ShowDialog<TResult>(IDialog<TResult> content)
    {
        var dialog = new DialogWindow
        {
            Content = content
        };
        
        await dialog.ShowDialog(mainWindow);

        return content.GetResult();
    }
}