using Chip_8.CustomControls;
using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chip_8.ViewModels;

public partial class InterpreterViewModel : ViewModelBase
{
    private Interpreter _cpu;

    [ObservableProperty] private Pixel[] _pixelBuffer = new Pixel[2048];
    
    public InterpreterViewModel(InterpreterOptions options)
    {
        _cpu = new Interpreter(options);

        for (int i = 0; i < PixelBuffer.Length; i++)
        {
            PixelBuffer[i] = new Pixel();
        }
    }
}