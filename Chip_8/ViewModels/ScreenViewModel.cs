using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chip_8.ViewModels;

partial class ScreenViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<bool>? _buffer;
    
    private const int BufferSize = 2048;
}