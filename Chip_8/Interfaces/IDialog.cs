namespace Chip_8.Interfaces;

public interface IDialog<out TResult>
{
    TResult GetResult();
}