using System;

namespace Chip_8.CustomControls;

public class Pixel
{
    public bool IsOn { get; private set; }

    public event Action? Changed;

    public void Flip(out bool turnedOff)
    {
        turnedOff = IsOn;
        
        IsOn = !IsOn;

        Changed?.Invoke();
    }
    
    public void Clear()
    {
        IsOn = false;
        
        Changed?.Invoke();
    }
}