using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Timers;

namespace Chip_8.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private byte[] _buffer = new byte[64 * 32];

        private Timer _timer;

        public MainWindowViewModel()
        {
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    int i = y * 64 + x;
                    Buffer[i] = (x + y) % 2 == 0 ? (byte)255 : (byte)0;
                }
            }
            
            _timer = new Timer(TimeSpan.FromMilliseconds(1000));
            _timer.Elapsed += Alternate;
            _timer.Start();
        }

        private void Alternate(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] = (Buffer[i] == 255) ? (byte)0 : (byte)255;
            }
        }
    }
}