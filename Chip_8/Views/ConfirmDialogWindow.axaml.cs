using Avalonia.Controls;
using Avalonia.Interactivity;
using Chip_8.ViewModels;

namespace Chip_8.Views;

public partial class ConfirmDialogWindow : Window
{
    public ConfirmDialogWindow()
    {
        InitializeComponent();
    }

    private void OnConfirm(object? sender, RoutedEventArgs args)
    {
        if (DataContext is ConfirmDialogViewModel vm)
            Close(vm.DialogContent.OnConfirm());
    }
    
    private void OnCancel(object? sender, RoutedEventArgs args)
    {
        if (DataContext is ConfirmDialogViewModel vm)
            Close(vm.DialogContent.OnCancel());
    }
}