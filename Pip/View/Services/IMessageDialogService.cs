namespace Pip.UI.View.Services;

public interface IMessageDialogService
{
    MessageDialogResult ShowOkCancelDialog(string content, string title);
}

public enum MessageDialogResult
{
    OK,
    Cancel
}
