namespace Chip_8.Data;

public class DisplayBuffer
{
    public const int Width = Chip8.DisplayWidth;
    public const int Height = Chip8.DisplayHeight;
    public bool IsDirty { get; set; }
    public bool[] Buffer { get; } = new bool[Width * Height];

    public bool XorPixel(int xPos, int yPos)
    {
        int index = yPos * Width + xPos;
        
        Buffer[index] = !Buffer[index];

        IsDirty = true;
        
        return !Buffer[index];
    }

    public void Clear()
    {
        for (int i = 0; i < Buffer.Length; i++)
            Buffer[i] = false;
        
        IsDirty = true;
    }
}