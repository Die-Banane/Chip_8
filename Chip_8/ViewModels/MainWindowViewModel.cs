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
        //set the current View to the main view
        CurrentView = currentView;
        
        //initialize Keyboard to route keypresses to the InterpreterView
        Keyboard = keyboard;
        
        //show the interpreter view and create it with the InterpreterOptions object that is being sent in the messge
        WeakReferenceMessenger.Default.Register<InitializeInterpreterMessage>(this, (_, m) =>
        {
            CurrentView = new InterpreterViewModel(m.Value, interpreterService);
        });
    }
}