using System.Collections.ObjectModel;
using Pip.Model;

namespace Pip.UI.ViewModel;

public interface ITreasuriesViewModel
{
    ObservableCollection<Treasury> Treasuries { get; }
    Treasury? SelectedTreasury { get; set; }
}
