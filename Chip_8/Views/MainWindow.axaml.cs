using Avalonia.Controls;
using Avalonia.Threading;
using System;

namespace Chip_8.Views
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}