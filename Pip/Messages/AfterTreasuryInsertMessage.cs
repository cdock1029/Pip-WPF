using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Pip.UI.Messages;

public class AfterTreasuryInsertMessage(AfterTreasuryInsertArgs value)
    : ValueChangedMessage<AfterTreasuryInsertArgs>(value);

public readonly record struct AfterTreasuryInsertArgs(string Cusip, DateOnly IssueDate);
