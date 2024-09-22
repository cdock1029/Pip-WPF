using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Pip.UI.Messages;

public class AfterInvestmentDeleteMessage(AfterInvestmentDeleteArgs value)
	: ValueChangedMessage<AfterInvestmentDeleteArgs>(value);

public readonly record struct AfterInvestmentDeleteArgs(string Cusip, DateOnly IssueDate);
