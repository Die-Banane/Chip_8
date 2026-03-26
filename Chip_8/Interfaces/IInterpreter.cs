using System.Threading.Tasks;

namespace Chip_8.Interfaces;

public interface IInterpreter
{
    Task RunAsync();
    Task StopAsync();
}