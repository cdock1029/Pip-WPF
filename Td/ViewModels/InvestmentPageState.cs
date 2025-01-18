namespace Td.ViewModels;

public class InvestmentPageState(ITreasuryDataProvider treasuryDataProvider) : BaseStateContainer
{
	public IEnumerable<Investment> Investments
	{
		get;
		private set
		{
			field = value;
			NotifyStateChanged();
		}
	} = null!;

	public override void LoadData()
	{
		Investments = treasuryDataProvider.GetInvestments();
	}

	public void DeleteInvestment(Investment investment)
	{
		treasuryDataProvider.Delete(investment);
	}

	public void Update(Investment investment)
	{
		treasuryDataProvider.Update(investment);
	}

	public void Create(Investment investment)
	{
		treasuryDataProvider.Add(investment);
	}
}