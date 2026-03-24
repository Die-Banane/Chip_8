using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Chip_8.ViewModels;

namespace Chip_8.Views;

public partial class InterpreterView : UserControl
{
    public InterpreterView()
    {
        InitializeComponent();
        
        DataContextChanged += async (_, _) =>
        {
            await StartInterpreterAsync();
        };
    }

    private async Task StartInterpreterAsync() =>
        await (DataContext as InterpreterViewModel)!.StartExecutionAsync().ConfigureAwait(false);
}