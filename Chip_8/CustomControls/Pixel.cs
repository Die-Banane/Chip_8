using System;
using Avalonia.Threading;

namespace Chip_8.CustomControls;

public class Pixel
{
    public bool IsOn { get; private set; }

    public event Action? Changed;

    public void Flip(out bool turnedOff)
    {
        turnedOff = IsOn;
        
        IsOn = !IsOn;

        Dispatcher.UIThread.Post(() => Changed?.Invoke());
    }
    
    public void Clear()
    {
        IsOn = false;
        
        Dispatcher.UIThread.Post(() => Changed?.Invoke());
    }
}