using Chip_8.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chip_8.ViewModels;

partial class ConfirmDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private IConfirmDialogContent _dialogContent;

    [ObservableProperty] private string _confirmText;
    [ObservableProperty] private string _cancelText;

    public ConfirmDialogViewModel(IConfirmDialogContent dialogContent)
    {
        DialogContent = dialogContent;

        if (DialogContent.GetConfirmText() is { } confirmText)
        {
            ConfirmText = confirmText;
        }
        else
        {
            ConfirmText = "Confirm";
        }
        
        if (DialogContent.GetCancelText() is { } cancelText)
        {
            CancelText = cancelText;
        }
        else
        {
            CancelText = "Cancel";
        }
    }
}