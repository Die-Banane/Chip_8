using Chip_8.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Chip_8.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private byte[] _buffer = Display.Buffer;

        private Interpreter i;

        public MainWindowViewModel()
        {
            i = new Interpreter();

            i.Initialize("C:\\Users\\jonas\\Downloads\\IBM Logo.ch8");
        }

        [RelayCommand]
        private void Execute()
        {
            Task.Run(() =>
            {
                i.Execute();
            });
        }
    }
}