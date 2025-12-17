using System;
using System.Collections.Generic;
using Avalonia.Input;

namespace Chip_8.Services;

public class Keyboard
{
    private const byte InvalidKey = 0xff;
    
    public Dictionary<Key, byte>? KeyMap { get; set; }
    
    private readonly HashSet<byte> _activeKeys = new();

    private byte _lastPressedKey = InvalidKey;

    public void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (KeyMap is null) return;

        if (KeyMap.TryGetValue(e.Key, out var key))
        {
            _activeKeys.Add(key);
            _lastPressedKey = InvalidKey;
            Console.WriteLine($"The Key {e.Key} with the corresponding Value {key} is down");
        }
    }

    public void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (KeyMap is null) return;

        if (KeyMap.TryGetValue(e.Key, out var key))
        {
            _activeKeys.Remove(key);
            _lastPressedKey = key;
            Console.WriteLine($"The Key {e.Key} with the corresponding Value {key} was released");
        }
    }
    
    //TODO: Fix this
    public bool TryConsumeLastPressedKey(out byte key)
    {
        if (_lastPressedKey == InvalidKey)
        {
            key = InvalidKey;
            return false;
        }

        key = _lastPressedKey;
        _lastPressedKey = InvalidKey;
        return true;
    }
    
    public bool IsKeyDown(byte key) => _activeKeys.Contains(key);
}