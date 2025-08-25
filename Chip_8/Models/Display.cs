namespace Chip_8.Models;

internal static class Display
{
    public static int Width { get; } = 64;
    public static int Height { get; } = 32;

    public static byte[] Buffer { get; set; } = new byte[Width * Height];
}