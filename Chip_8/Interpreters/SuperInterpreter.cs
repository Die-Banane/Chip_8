using System;
using System.Collections.Generic;
using Avalonia.Input;
using Chip_8.Interfaces;

namespace Chip_8.Interpreters;

public class SuperInterpreter : IInterpreter
{
    public Dictionary<Key, byte>? KeyMap { get; set; }

    public void Run()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}