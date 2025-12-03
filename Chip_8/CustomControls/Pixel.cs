using System;

namespace Chip_8.CustomControls;

public class Pixel
{
    public bool IsOn { get; private set; }

    public event Action? Changed;

    public void Flip()
    {
        IsOn = !IsOn;

        Changed?.Invoke();
    }
}