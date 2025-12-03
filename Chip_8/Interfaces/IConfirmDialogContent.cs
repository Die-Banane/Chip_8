namespace Chip_8.Interfaces;

public interface IConfirmDialogContent
{
    object? OnConfirm();
    object? OnCancel();
    
    string? GetConfirmText();
    string? GetCancelText();
}