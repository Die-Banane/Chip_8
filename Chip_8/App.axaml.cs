using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Chip_8.Interfaces;
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
        collection.AddSingleton<IDialogService, DialogService>();
        collection.AddSingleton<InterpreterService>();
        collection.AddSingleton<Keyboard>();
        collection.AddTransient<MainViewModel>();
        collection.AddSingleton<MainWindow>();
        
        var serviceProvider = collection.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}