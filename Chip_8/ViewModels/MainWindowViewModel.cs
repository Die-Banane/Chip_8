using Chip_8.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chip_8.ViewModels;

partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentView;
    
    public Keyboard Keyboard { get; }

    public MainWindowViewModel(MainViewModel mainViewModel, Keyboard keyboard, NavigationService navigationService)
    {
        CurrentView = mainViewModel;
        
        //initialize Keyboard to route keypresses to the InterpreterView
        Keyboard = keyboard;
        
        navigationService.CurrentViewChanged += vm => CurrentView = vm;
        navigationService.NavigateTo(mainViewModel);
    }
}
