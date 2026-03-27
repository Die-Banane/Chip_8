namespace Chip_8.Data;

public class DialogResult<T>
{
    public bool Confirmed { get; }
    public T? Value { get; }

    private DialogResult(bool confirmed, T? value)
    {
        Confirmed = confirmed;
        Value = value;
    }

    public static DialogResult<T> Ok(T value) => new(true, value);
    public static DialogResult<T> Cancelled() => new(false, default);
}