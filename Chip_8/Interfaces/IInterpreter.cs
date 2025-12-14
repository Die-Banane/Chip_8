using System;
using System.Collections.Generic;
using Avalonia.Input;

namespace Chip_8.Interfaces;

public interface IInterpreter : IDisposable
{
    public Dictionary<Key, byte>? KeyMap { get; }

    public void Run();
}