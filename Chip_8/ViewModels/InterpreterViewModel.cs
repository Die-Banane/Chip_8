using System;
using System.Threading.Tasks;
using Chip_8.CustomControls;
using Chip_8.Interfaces;
using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chip_8.ViewModels;

public partial class InterpreterViewModel : ViewModelBase
{
    [ObservableProperty] private Pixel[] _pixelBuffer = new Pixel[2048];
    
    private IInterpreter? _cpu;
    
    public InterpreterViewModel(InterpreterOptions options, InterpreterService interpreterService)
    {
        for (int i = 0; i < PixelBuffer.Length; i++)
        {
            PixelBuffer[i] = new Pixel();
        }
        
        _cpu = interpreterService.CreateInterpreter(options, PixelBuffer);
        
        Task.Run(() => _cpu.Run());
    }
}