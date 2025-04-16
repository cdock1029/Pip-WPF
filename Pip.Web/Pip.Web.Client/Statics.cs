namespace Pip.Web.Client;

public static class Statics
{
    public const string DateFormatString = "yyyy MMM dd";

    public const string GridFont =
        "font-family: Consolas,'Cascadia Mono',monospace; font-variant-numeric: tabular-nums oldstyle-nums; font-variant-caps: normal; text-transform: uppercase;";

    public static readonly GridSort<TreasuryItemViewModel> TermSpanSort = GridSort<TreasuryItemViewModel>
        .ByAscending(t => t.TermDaysSpan);
}