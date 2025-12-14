using Chip_8.Services;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Chip_8.ViewModels;

partial class MainViewModel(DialogFactory factory) : ViewModelBase
{
    [RelayCommand]
    private async Task SetUpInterpreter(string? path = null)
    {
        var response = await factory.CreateConfirmDialog<SettingsViewModel>(null, factory, path!);
        
        if (response is not null)
            WeakReferenceMessenger.Default.Send(new InitializeInterpreterMessage((InterpreterOptions)response));
    }
}

public sealed class InitializeInterpreterMessage(InterpreterOptions value) : ValueChangedMessage<InterpreterOptions>(value);