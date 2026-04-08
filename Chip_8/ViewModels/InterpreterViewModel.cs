using System.Threading.Tasks;
using Chip_8.Data;
using Chip_8.Interfaces;
using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Chip_8.ViewModels;

public partial class InterpreterViewModel : ViewModelBase
{
    [ObservableProperty] private DisplayBuffer _displayBuffer = new();
    
    private readonly IInterpreter _cpu;
    private readonly NavigationService _navigationService;

    public InterpreterViewModel(InterpreterOptions options, InterpreterService interpreterService, NavigationService navigationService)
    {
        _navigationService = navigationService;
        
        _cpu = interpreterService.BuildInterpreter(options, DisplayBuffer);
        _ = _cpu.RunAsync().ContinueWith(t =>
        {
            if (t.IsFaulted)
                throw t.Exception;
        });
    }
    
    [RelayCommand]
    private async Task CloseInterpreterAsync()
    {
        await _cpu.StopAsync();
        
        _navigationService.GoBack();
    }
}