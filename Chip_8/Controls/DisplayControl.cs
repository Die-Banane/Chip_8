using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using System;

namespace Chip_8.Controls
{
    internal class DisplayControl : Control
    {
        private DispatcherTimer _timer;
        
        public static StyledProperty<byte[]> BufferProperty =
            AvaloniaProperty.Register<DisplayControl, byte[]>(nameof(Buffer));

        public byte[] Buffer
        {
            get => GetValue(BufferProperty);
            set => SetValue(BufferProperty, value);
        }

        public int HeightPixels { get; set; } = 32;
        public int WidthPixels { get; set; } = 64;
        public int RefreshRate { get; set; } = 60;

        public DisplayControl()
        {
            _timer = new() { Interval = TimeSpan.FromMilliseconds(1000 / RefreshRate) };
            _timer.Tick += Refresh;
            _timer.Start();
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (Buffer == null)
                return;

            double scaleX = Bounds.Width / WidthPixels;
            double scaleY = Bounds.Height / HeightPixels;

            for (int y = 0; y < HeightPixels; y++)
            {
                for (int x = 0; x < WidthPixels; x++)
                {
                    int i = y * WidthPixels + x;
                    if (Buffer[i] != 0)
                    {
                        var pixel = new Rect(x * scaleX, y * scaleY, scaleX, scaleY);
                        context.FillRectangle(Brushes.White, pixel);
                    }
                    else
                    {
                        var pixel = new Rect(x * scaleX, y * scaleY, scaleX, scaleY);
                        context.FillRectangle(Brushes.Black, pixel);
                    }
                }
            }
        }

        private void Refresh(object sender, EventArgs e)
        {
            InvalidateVisual();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            _timer.Stop();
        }
    }
}