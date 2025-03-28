using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Pip.UI.Components.Investments.Messages;

public class AfterDeleteInvestmentMessage(AfterInvestmentDeleteArgs value)
    : ValueChangedMessage<AfterInvestmentDeleteArgs>(value);

public readonly record struct AfterInvestmentDeleteArgs(string Cusip, DateOnly IssueDate);