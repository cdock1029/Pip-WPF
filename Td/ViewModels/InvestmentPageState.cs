namespace Td.ViewModels;

public class InvestmentPageState(ITreasuryService treasuryService) : BaseStateContainer
{
	public List<Investment> Investments
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
		Investments = treasuryService.GetInvestments();
	}

	public void DeleteInvestment(Investment investment)
	{
		treasuryService.Delete(investment);
	}

	public void Update(Investment investment)
	{
		treasuryService.Update(investment);
	}

	public void Create(Investment investment)
	{
		treasuryService.Add(investment);
	}
}