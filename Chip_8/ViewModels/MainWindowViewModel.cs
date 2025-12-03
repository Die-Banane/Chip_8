using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Chip_8.ViewModels;

partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentView;

    public MainWindowViewModel(MainViewModel currentView)
    {
        CurrentView = currentView;
        
        WeakReferenceMessenger.Default.Register<InitializeInterpreterMessage>(this, (_, m) =>
        {
            CurrentView = new InterpreterViewModel(m.Value);
        });
    }
}