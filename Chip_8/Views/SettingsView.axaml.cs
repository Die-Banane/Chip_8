using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Chip_8.ViewModels;

namespace Chip_8.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (TopLevel.GetTopLevel(this) is Window window) //prevent Window from closing with alt + f4
            window.Closing += (_, args) =>
            {
                args.Cancel = true;
            };
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        
        if (DataContext is SettingsViewModel vm)
            vm.Path = string.Empty; //set Path to string.Empty so that GetResult returns InvalidOptions
        
        window?.Closing += (_, args) =>
        {
            args.Cancel = false;
        };
        
        window?.Close();
    }

    private void Run_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = TopLevel.GetTopLevel(this) as Window;
        
        window?.Closing += (_, args) =>
        {
            args.Cancel = false;
        };
        
        window?.Close();
    }
}