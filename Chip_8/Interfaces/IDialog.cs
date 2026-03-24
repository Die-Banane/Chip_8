using Chip_8.Data;

namespace Chip_8.Interfaces;

public interface IDialog<TResult>
{
    DialogResult<TResult> GetResult();
}