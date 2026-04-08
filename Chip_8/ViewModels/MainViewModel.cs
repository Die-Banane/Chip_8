using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Chip_8.Interfaces;
using Chip_8.Services;
using Chip_8.Views;

namespace Chip_8.ViewModels;

public partial class MainViewModel(IDialogService dialogService, MainWindow mainWindow, InterpreterService interpreterService, NavigationService navigationService) : ViewModelBase
{
    [RelayCommand]
    private async Task SetUpInterpreter(string? path = null)
    {
        var result = await dialogService.ShowDialog(new SettingsViewModel(mainWindow, path));

        if (result.Confirmed)
        {
            var interpreterVm = new InterpreterViewModel(result.Value!, interpreterService, navigationService);
            
            navigationService.NavigateTo(interpreterVm);
        }
    }
}