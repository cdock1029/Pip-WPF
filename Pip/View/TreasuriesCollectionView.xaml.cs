using System.Windows;
using DevExpress.Mvvm;
using Pip.Model;

namespace Pip.UI.View;

public partial class TreasuriesCollectionView
{
	public static readonly DependencyProperty TreasuriesProperty = DependencyProperty.Register(
		nameof(Treasuries), typeof(IEnumerable<Treasury>), typeof(TreasuriesCollectionView),
		new PropertyMetadata(default(IEnumerable<Treasury>)));

	public static readonly DependencyProperty SelectedTreasuryProperty = DependencyProperty.Register(
		nameof(SelectedTreasury), typeof(Treasury), typeof(TreasuriesCollectionView),
		new PropertyMetadata(default(Treasury)));

	public static readonly DependencyProperty HandleSelectionChangedCommandProperty = DependencyProperty.Register(
		nameof(HandleSelectionChangedCommand), typeof(DelegateCommand<Treasury>), typeof(TreasuriesCollectionView),
		new PropertyMetadata(default(DelegateCommand<Treasury>)));

	public TreasuriesCollectionView()
	{
		InitializeComponent();
	}

	public Treasury SelectedTreasury
	{
		get => (Treasury)GetValue(SelectedTreasuryProperty);
		set => SetValue(SelectedTreasuryProperty, value);
	}

	public IEnumerable<Treasury> Treasuries
	{
		get => (IEnumerable<Treasury>)GetValue(TreasuriesProperty);
		set => SetValue(TreasuriesProperty, value);
	}

	public DelegateCommand<Treasury> HandleSelectionChangedCommand
	{
		get => (DelegateCommand<Treasury>)GetValue(HandleSelectionChangedCommandProperty);
		set => SetValue(HandleSelectionChangedCommandProperty, value);
	}
}