using CommunityToolkit.Mvvm.ComponentModel;

namespace Pip.UI.ViewModel;

public class ViewModelBase : ObservableObject
{
    public virtual Task LoadAsync()
    {
        return Task.CompletedTask;
    }
}
