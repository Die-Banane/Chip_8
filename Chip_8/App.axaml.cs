using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Chip_8.Services;
using Chip_8.ViewModels;
using Chip_8.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chip_8;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();

        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<DialogFactory>();
        collection.AddSingleton<InterpreterService>();
        collection.AddTransient<ConfirmDialogWindow>();
        collection.AddTransient<MainViewModel>();
        
        var serviceProvider = collection.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}