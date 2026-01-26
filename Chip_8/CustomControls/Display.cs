using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Chip_8.Data;

namespace Chip_8.CustomControls;

public class Display : Control
{
    public static readonly StyledProperty<DisplayBuffer> DisplayBufferProperty =
        AvaloniaProperty.Register<Display, DisplayBuffer>(nameof(DisplayBuffer));

    public DisplayBuffer DisplayBuffer
    {
        get => GetValue(DisplayBufferProperty);
        set => SetValue(DisplayBufferProperty, value);
    }

    private readonly WriteableBitmap _frame = new(new PixelSize(DisplayBuffer.Width, DisplayBuffer.Height),
        new Vector(96, 96),
        PixelFormat.Bgra8888);

    private readonly DispatcherTimer timer = new()
    {
        Interval = TimeSpan.FromMilliseconds(16.6)
    };

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        RenderOptions.SetBitmapInterpolationMode(this, BitmapInterpolationMode.None);
        
        timer.Tick += (_, _) =>
        {
            if (DisplayBuffer.IsDirty)
            {
                InvalidateVisual();
                DisplayBuffer.IsDirty = false;
            }
        };

        timer.Start();

        base.OnAttachedToVisualTree(e);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        timer.Stop();
        
        base.OnDetachedFromVisualTree(e);
    }

    public override void Render(DrawingContext context)
    {
        using (var framebuffer = _frame.Lock())
        {
            unsafe
            {
                byte* address = (byte*)framebuffer.Address;

                for (int y = 0; y < DisplayBuffer.Height; y++)
                {
                    for (int x = 0; x < DisplayBuffer.Width; x++)
                    {
                        byte* pixel = address + y * framebuffer.RowBytes + x * 4;

                        byte color = (byte)(DisplayBuffer.Buffer[y * DisplayBuffer.Width + x] ? 255 : 0);

                        pixel[0] = color;
                        pixel[1] = color;
                        pixel[2] = color;
                        pixel[3] = 255;
                    }
                }
            }
        }

        context.DrawImage(_frame,
            new Rect(0, 0, _frame.PixelSize.Width, _frame.PixelSize.Height),
            new Rect(0, 0, Bounds.Width, Bounds.Height));

        base.Render(context);
    }
}