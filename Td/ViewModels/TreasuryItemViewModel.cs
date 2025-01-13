namespace Td.ViewModels;

public class TreasuryItemViewModel
{
	public string? Cusip { get; init; }

	public DateOnly? IssueDate { get; init; }

	public DateOnly? MaturityDate { get; init; }

	public DateOnly? AuctionDate { get; init; }

	public TreasuryType? Type { get; init; }

	public string? Term { get; init; }

	public string Value => $"{Cusip}_{IssueDate}";

	public int? TermDaysSpan => IssueDate is not null && MaturityDate is not null
		? MaturityDate.Value.DayNumber - IssueDate.Value.DayNumber
		: null;
}