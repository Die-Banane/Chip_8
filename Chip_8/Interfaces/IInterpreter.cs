using System;
using System.Threading.Tasks;

namespace Chip_8.Interfaces;

public interface IInterpreter : IDisposable
{
    public void Run();
}