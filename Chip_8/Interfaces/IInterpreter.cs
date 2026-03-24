using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Input;

namespace Chip_8.Interfaces;

public interface IInterpreter
{
    public Dictionary<Key, byte>? KeyMap { get; }

    Task RunAsync();
    Task StopAsync();
}