using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Chip_8.ViewModels;

partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentView;
    [ObservableProperty] private Keyboard _keyboard;

    public MainWindowViewModel(MainViewModel currentView, Keyboard keyboard, InterpreterService interpreterService)
    {
        CurrentView = currentView;
        Keyboard = keyboard;
        
        WeakReferenceMessenger.Default.Register<InitializeInterpreterMessage>(this, (_, m) =>
        {
            CurrentView = new InterpreterViewModel(m.Value, interpreterService);
        });
    }
}