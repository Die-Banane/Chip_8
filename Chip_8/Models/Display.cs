using System;
using System.Timers;

namespace Chip_8.Models;

internal class Display
{
    public byte[] Buffer { get; set; } = new byte[64 * 32];
}