using Microsoft.EntityFrameworkCore;
using Pip.Model;

namespace Pip.DataAccess;

public class PipDbContext(DbContextOptions<PipDbContext> options) : DbContext(options)
{
    public DbSet<Treasury> Treasuries { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        List<Treasury> treasuries =
        [
            new Treasury
            {
                Cusip = "912797GL5",
                IssueDate = new DateOnly(2024, 7, 25),
                MaturityDate = new DateOnly(2024, 9, 5),
                SecurityType = TreasurySecurityType.Bill, SecurityTerm = "42-Day", Type = TreasuryType.Bill
            },
            new Treasury
            {
                Cusip = "912797KX4",
                IssueDate = new DateOnly(2024, 6, 18),
                MaturityDate = new DateOnly(2024, 8, 13),
                SecurityType = TreasurySecurityType.Bill, SecurityTerm = "8-Week", Type = TreasuryType.Bill
            },
            new Treasury
            {
                Cusip = "912797GK7",
                IssueDate = new DateOnly(2024, 5, 9),
                MaturityDate = new DateOnly(2024, 8, 8),
                SecurityType = TreasurySecurityType.Bill, SecurityTerm = "13-Week", Type = TreasuryType.Bill
            },
            new Treasury
            {
                Cusip = "912797GL5",
                IssueDate = new DateOnly(2024, 6, 6),
                MaturityDate = new DateOnly(2024, 9, 5),
                SecurityType = TreasurySecurityType.Bill, SecurityTerm = "13-Week", Type = TreasuryType.Bill
            }
        ];

        List<Investment> investments =
        [
            new Investment
            {
                Id = 1,
                Confirmation = "FOO",
                Par = 100_000,
                TreasuryCusip = treasuries[0].Cusip,
                TreasuryIssueDate = treasuries[0].IssueDate
            },
            new Investment
            {
                Id = 2,

                Confirmation = "BAR",
                Par = 55_000,
                TreasuryCusip = treasuries[1].Cusip,
                TreasuryIssueDate = treasuries[1].IssueDate
            },
            new Investment
            {
                Id = 3,
                Confirmation = "BAZ",
                Par = 2000_400,
                TreasuryCusip = treasuries[2].Cusip,
                TreasuryIssueDate = treasuries[2].IssueDate
            }
        ];

        modelBuilder.Entity<Treasury>().HasData(treasuries);
        modelBuilder.Entity<Investment>().HasData(investments);
    }
}
