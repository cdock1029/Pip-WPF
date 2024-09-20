using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Pip.UI.Messages;

public class AfterInsertInvestmentMessage(AfterInsertInvestmentArgs value)
	: ValueChangedMessage<AfterInsertInvestmentArgs>(value);

public readonly record struct AfterInsertInvestmentArgs(int Id, string Cusip, DateOnly IssueDate);
