using System.Threading;
using System.Threading.Tasks;
using Chip_8.Data;
using Chip_8.Interfaces;
using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Chip_8.ViewModels;

public partial class InterpreterViewModel : ViewModelBase
{
    [ObservableProperty] private DisplayBuffer _displayBuffer = new();
    
    private readonly InterpreterService _interpreterService;
    private readonly InterpreterOptions _options;
    
    public InterpreterViewModel(InterpreterOptions options, InterpreterService interpreterService)
    {
        _interpreterService = interpreterService;
        _options = options;
    }

    public async Task StartExecutionAsync()
    {
        IInterpreter cpu = await _interpreterService.CreateInterpreterAsync(_options, DisplayBuffer);
        await cpu.RunAsync().ConfigureAwait(false);
    }
    
    [RelayCommand]
    private async Task CloseInterpreterAsync()
    {
        await _interpreterService.StopCurrentAsync().ConfigureAwait(false);
        
        WeakReferenceMessenger.Default.Send(new InterpreterDisposedMessage());
    }
}

public sealed class InterpreterDisposedMessage { }