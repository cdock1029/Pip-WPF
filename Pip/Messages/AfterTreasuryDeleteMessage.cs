using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Pip.UI.Messages;

public class AfterTreasuryDeleteMessage(AfterTreasuryDeleteArgs value)
	: ValueChangedMessage<AfterTreasuryDeleteArgs>(value);

public readonly record struct AfterTreasuryDeleteArgs(string Cusip, DateOnly? IssueDate);
