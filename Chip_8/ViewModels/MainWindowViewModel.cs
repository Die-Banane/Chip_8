using Chip_8.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Chip_8.Services;

namespace Chip_8.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private byte[] _buffer = Display.Buffer;

        [ObservableProperty]
        private string? _program;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExecuteCommand))]
        private bool _canRun;

        private IDialogService _dialogService;
        private Interpreter _chip8;

        public MainWindowViewModel()
        {   
            _chip8 = new Interpreter();
            _dialogService = new DialogService();
        }

        [RelayCommand(CanExecute = nameof(CanRun))]
        private async Task Execute()
        {   
            if (Program is not null)
            {
                CanRun = false;
                _chip8?.Initialize(Program);
                await Task.Run(_chip8!.Execute);
            }
        }

        [RelayCommand]
        private async Task OpenFileAsync()
        {
            Program = await _dialogService.ShowFileDialog();
            CanRun = Program is not null;
        }
    }
}