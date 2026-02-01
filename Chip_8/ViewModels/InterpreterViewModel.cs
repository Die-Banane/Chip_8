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
    
    public InterpreterViewModel(InterpreterOptions options, InterpreterService interpreterService)
    {
        _interpreterService = interpreterService;
        IInterpreter cpu = _interpreterService.CreateInterpreter(options, _displayBuffer);
        
        Task.Run(() => cpu.Run());
    }

    [RelayCommand]
    private void CloseInterpreter()
    {
        _interpreterService.CurrentInstance?.Dispose();

        WeakReferenceMessenger.Default.Send(new InterpreterDisposedMessage());
    }
}

public sealed class InterpreterDisposedMessage { }