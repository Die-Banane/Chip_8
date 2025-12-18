using System;
using System.Collections.Generic;
using Avalonia.Input;

namespace Chip_8.Services;

public class Keyboard
{
    public static readonly byte InvalidKey = 0xff;

    private readonly HashSet<byte> _activeKeys = new();

    public bool WaitingForKey { get; set; }
    
    public Dictionary<Key, byte>? KeyMap { get; set; }

    public byte PendingKey { get; set; } = InvalidKey;

    public void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (KeyMap is null) return;

        if (KeyMap.TryGetValue(e.Key, out var key))
        {
            _activeKeys.Add(key);
            Console.WriteLine($"The Key {e.Key} with the corresponding Value {key} is down");
        }
    }

    public void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (KeyMap is null) return;

        if (KeyMap.TryGetValue(e.Key, out var key))
        {
            if (WaitingForKey)
            {
                PendingKey = key;
                WaitingForKey = false;
            }

            _activeKeys.Remove(key);
        }
    }
    
    public bool IsKeyDown(byte key) => _activeKeys.Contains(key);
}