using Chip_8.Services;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Chip_8.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Chip_8.ViewModels;

partial class MainViewModel(FilePickerFactory filePickerFactory, IDialogService dialogService) : ViewModelBase
{
    [RelayCommand]
    private async Task SetUpInterpreter(string? path = null)
    {
        var result = await dialogService.ShowDialog(new SettingsViewModel(filePickerFactory, path));
        
        if (result != InterpreterOptions.InvalidOptions)
            WeakReferenceMessenger.Default.Send(new InitializeInterpreterMessage(result));
    }
}

public sealed class InitializeInterpreterMessage(InterpreterOptions value) : ValueChangedMessage<InterpreterOptions>(value);