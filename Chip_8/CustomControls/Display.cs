using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Chip_8.CustomControls;

public class Display : Control
{
    public static readonly StyledProperty<Pixel[]> BufferProperty = 
        AvaloniaProperty.Register<Display, Pixel[]>(
            nameof(Buffer),
            new Pixel[2048]);

    public Pixel[] Buffer
    {
        get => GetValue(BufferProperty);
        set => SetValue(BufferProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        
        context.PushRenderOptions(new RenderOptions
        {
            EdgeMode = EdgeMode.Aliased,
        });
        
        foreach (var pixel in Buffer)
        {
            pixel.Changed += InvalidateVisual;
        }

        double pixelSize = Math.Min(Bounds.Width / 64, Bounds.Height / 32);
        
        double displayHeight = pixelSize * 32;
        double displayWidth = pixelSize * 64;
        
        double offsetX = (Bounds.Width - displayWidth) / 2;
        double offsetY = (Bounds.Height - displayHeight) / 2;

        for (int i = 0; i < Buffer.Length; i++) 
        {
            int x = i % 64;
            int y = i / 64;
            
            var size = new Size(pixelSize, pixelSize);
            var position = new Point(pixelSize * x + offsetX, pixelSize * y + offsetY); 
                
            context.FillRectangle(Buffer[i].IsOn ? Brushes.White : Brushes.Black, new Rect(position, size));
        }
    }
}