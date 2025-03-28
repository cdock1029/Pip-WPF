using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Pip.UI.Components.Investments.Messages;

public class AfterInsertInvestmentMessage(AfterInsertInvestmentArgs value)
    : ValueChangedMessage<AfterInsertInvestmentArgs>(value);

public readonly record struct AfterInsertInvestmentArgs(int Id);