using System.Threading.Tasks;
using Chip_8.Data;

namespace Chip_8.Interfaces;

public interface IDialogService
{
    public Task<DialogResult<TResult>> ShowDialog<TResult>(IDialog<TResult> content);
}