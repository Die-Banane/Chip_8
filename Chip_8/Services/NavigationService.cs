using System;
using Chip_8.ViewModels;

namespace Chip_8.Services;

public class NavigationService
{
    private ViewModelBase? _lastPage;

    private ViewModelBase? _currentView;

    public event Action<ViewModelBase>? CurrentViewChanged;

    public void NavigateTo(ViewModelBase viewModel)
    {
        _lastPage = _currentView;
        _currentView = viewModel;
        CurrentViewChanged?.Invoke(viewModel);
    }

    public void GoBack()
    {
        _currentView = _lastPage ?? throw new InvalidOperationException("no previous page detected");
        CurrentViewChanged?.Invoke(_lastPage); 
    }
}