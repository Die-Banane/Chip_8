using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Chip_8.Interfaces;
using Chip_8.Views;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Chip_8.ViewModels;

partial class MainViewModel(IDialogService dialogService, MainWindow mainWindow) : ViewModelBase
{
    [RelayCommand]
    private async Task SetUpInterpreter(string? path = null)
    {
        var result = await dialogService.ShowDialog(new SettingsViewModel(mainWindow, path));
        
        if (result.Confirmed)
            WeakReferenceMessenger.Default.Send(new InitializeInterpreterMessage(result.Value!));
            
    }
}

public sealed class InitializeInterpreterMessage(InterpreterOptions value) : ValueChangedMessage<InterpreterOptions>(value);