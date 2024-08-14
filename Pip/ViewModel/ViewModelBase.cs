using CommunityToolkit.Mvvm.ComponentModel;

namespace Pip.UI.ViewModel;

public class ViewModelBase : ObservableRecipient
{
    public virtual Task LoadAsync()
    {
        return Task.CompletedTask;
    }
}
