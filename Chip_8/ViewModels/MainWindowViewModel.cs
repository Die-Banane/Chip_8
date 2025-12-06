using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Chip_8.ViewModels;

partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentView;

    public MainWindowViewModel(MainViewModel currentView, InterpreterService interpreterService)
    {
        CurrentView = currentView;
        
        WeakReferenceMessenger.Default.Register<InitializeInterpreterMessage>(this, (_, m) =>
        {
            CurrentView = new InterpreterViewModel(m.Value, interpreterService);
        });
    }
}