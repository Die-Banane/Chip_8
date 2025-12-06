using System.Threading.Tasks;
using Chip_8.CustomControls;
using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chip_8.ViewModels;

public partial class InterpreterViewModel : ViewModelBase
{
    [ObservableProperty] private Pixel[] _pixelBuffer = new Pixel[2048];
    
    public InterpreterViewModel(InterpreterOptions options, InterpreterService interpreterService)
    {
        var cpu = interpreterService.CreateInterpreter(options, PixelBuffer);

        for (int i = 0; i < PixelBuffer.Length; i++)
        {
            PixelBuffer[i] = new Pixel();
        }

        Task.Run(() => cpu.Run());
    }
}