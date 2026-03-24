using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Chip_8.ViewModels;

namespace Chip_8.Views;

public partial class SettingsView : UserControl
{
    private bool _canClose;
    
    public SettingsView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (TopLevel.GetTopLevel(this) is Window window) //prevent Window from closing with alt + f4
            window.Closing += OnWindowClosing;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        
        if (TopLevel.GetTopLevel(this) is Window window)
            window.Closing -= OnWindowClosing;
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        _canClose = true;
        
        var window = TopLevel.GetTopLevel(this) as Window;
        
        if (DataContext is SettingsViewModel vm)
            vm.Cancel();
        
        window?.Close();
    }

    private void Run_OnClick(object? sender, RoutedEventArgs e)
    {
        _canClose = true;
        
        var window = TopLevel.GetTopLevel(this) as Window;
        
        window?.Close();
    }

    private void OnWindowClosing(object? sender, WindowClosingEventArgs e) => e.Cancel = !_canClose;
}