namespace Chip_8.Models;

internal static class Display
{
    public static byte[] Buffer { get; set; } = new byte[64 * 32];
}