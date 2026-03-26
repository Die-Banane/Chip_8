using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Input;
using Chip_8.Interfaces;

namespace Chip_8.Interpreters;

public class XoInterpreter : IInterpreter
{
    public Dictionary<Key, byte>? KeyMap { get; set; }

    public CancellationTokenSource Cts { get; }

    public async Task RunAsync()
    {
        throw new NotImplementedException();
    }

    public async Task StopAsync()
    {
        throw new NotImplementedException();
    }
}