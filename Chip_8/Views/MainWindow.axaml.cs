using Avalonia.Controls;
using Chip_8.ViewModels;

namespace Chip_8.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContextChanged += (_, _) =>
        {
            if (DataContext is MainWindowViewModel vm)
            {
                KeyDown += vm.Keyboard.OnKeyDown;
                KeyUp += vm.Keyboard.OnKeyUp;
            }
        };
    }
}