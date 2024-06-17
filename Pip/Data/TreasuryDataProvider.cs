using System.Diagnostics.CodeAnalysis;
using Pip.Model;

namespace Pip.Data;

public interface ITreasuryDataProvider
{
    Task<IEnumerable<Treasury>?> GetTreasuries();
}

public class TreasuryDataProvider : ITreasuryDataProvider
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public async Task<IEnumerable<Treasury>?> GetTreasuries()
    {
        await Task.Delay(100);
        return
        [
            new Treasury
                { Cusip = "ABCDEFG", IssueDate = new DateTime(2024, 7, 1), MaturityDate = new DateTime(2024, 8, 1) },
            new Treasury
                { Cusip = "XYZ", IssueDate = new DateTime(2024, 7, 15), MaturityDate = new DateTime(2024, 9, 2) },
            new Treasury
                { Cusip = "MNOP", IssueDate = new DateTime(2024, 8, 7), MaturityDate = new DateTime(2024, 10, 5) },
            new Treasury
                { Cusip = "QRS", IssueDate = new DateTime(2025, 1, 1), MaturityDate = new DateTime(2026, 1, 1) }
        ];
    }
}
