using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace Pip.UI.View.Services;

public class MessageDialogService : IMessageDialogService
{
	public MessageDialogResult ShowOkCancelDialog(string content, string title)
	{
		var result = MessageBox.Show(content, title, MessageBoxButton.OKCancel);
		return result == MessageBoxResult.OK ? MessageDialogResult.OK : MessageDialogResult.Cancel;
	}
}
