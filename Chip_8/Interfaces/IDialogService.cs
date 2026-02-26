using System.Threading.Tasks;

namespace Chip_8.Interfaces;

public interface IDialogService
{
    public Task<TResult> ShowDialog<TResult>(IDialog<TResult> content);
}