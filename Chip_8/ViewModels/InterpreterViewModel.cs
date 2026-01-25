using System.Threading.Tasks;
using Chip_8.Data;
using Chip_8.Interfaces;
using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chip_8.ViewModels;

public partial class InterpreterViewModel : ViewModelBase
{
    [ObservableProperty] private DisplayBuffer _displayBuffer = new();
    
    public InterpreterViewModel(InterpreterOptions options, InterpreterService interpreterService)
    {
        IInterpreter cpu = interpreterService.CreateInterpreter(options, _displayBuffer);
        
        Task.Run(() => cpu.Run());
    }
}