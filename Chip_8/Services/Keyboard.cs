using System;
using System.Collections.Generic;
using Avalonia.Input;

namespace Chip_8.Services;

public class Keyboard
{
    private const byte InvalidKey = 0xff;
    
    public Dictionary<Key, byte>? KeyMap { get; set; }
    
    private readonly HashSet<byte> _activeKeys = new();

    public byte LastPressedKey { get; private set; } = InvalidKey;

    public void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (KeyMap is null) return;

        if (KeyMap.TryGetValue(e.Key, out var key))
        {
            _activeKeys.Add(key);
            LastPressedKey = InvalidKey;
            Console.WriteLine($"The Key {e.Key} with the corresponding Value {key} is down");
        }
    }

    public void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (KeyMap is null) return;

        if (KeyMap.TryGetValue(e.Key, out var key))
        {
            _activeKeys.Remove(key);
            LastPressedKey = key;
            Console.WriteLine($"The Key {e.Key} with the corresponding Value {key} was released");
        }
        
    }
    
    public bool TryConsumeLastPressedKey(out byte key)
    {
        if (LastPressedKey == InvalidKey)
        {
            key = InvalidKey;
            return false;
        }

        key = LastPressedKey;
        LastPressedKey = InvalidKey;
        return true;
    }
    
    public bool IsKeyDown(byte key) => _activeKeys.Contains(key);
}