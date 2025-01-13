using Microsoft.EntityFrameworkCore;

namespace Td.Data;

public class TreasuryDirectorContext : DbContext
{
    public DbSet<Investment> Investments { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=td.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        List<Treasury> treasuries =
        [
            new()
            {
                Cusip = "912797GL5",
                AuctionDate = new DateOnly(2024, 7, 25),
                IssueDate = new DateOnly(2024, 7, 25),
                MaturityDate = new DateOnly(2024, 9, 5),
                SecurityType = TreasurySecurityType.Bill, SecurityTerm = "42-Day", Type = TreasuryType.Bill
            },
            new()
            {
                Cusip = "912797KX4",
                AuctionDate = new DateOnly(2024, 6, 18),
                IssueDate = new DateOnly(2024, 6, 18),
                MaturityDate = new DateOnly(2024, 8, 13),
                SecurityType = TreasurySecurityType.Bill, SecurityTerm = "8-Week", Type = TreasuryType.Bill
            },
            new()
            {
                Cusip = "912797GK7",
                AuctionDate = new DateOnly(2024, 5, 9),
                IssueDate = new DateOnly(2024, 5, 9),
                MaturityDate = new DateOnly(2024, 8, 8),
                SecurityType = TreasurySecurityType.Bill, SecurityTerm = "13-Week", Type = TreasuryType.Bill
            },
            new()
            {
                Cusip = "912797GL5",
                AuctionDate = new DateOnly(2024, 6, 6),
                IssueDate = new DateOnly(2024, 6, 6),
                MaturityDate = new DateOnly(2024, 9, 5),
                SecurityType = TreasurySecurityType.Bill, SecurityTerm = "13-Week", Type = TreasuryType.Bill
            }
        ];

        List<Investment> investments =
        [
            new()
            {
                Id = 1,
                Confirmation = "FOOXX",
                Par = 100_000,
                Cusip = treasuries[0].Cusip,
                AuctionDate = treasuries[0].AuctionDate,
                IssueDate = treasuries[0].IssueDate,
                MaturityDate = treasuries[0].MaturityDate,
                SecurityTerm = treasuries[0].SecurityTerm,
                Type = treasuries[0].Type
            },
            new()
            {
                Id = 2,

                Confirmation = "BARXX",
                Par = 55_000,
                Cusip = treasuries[1].Cusip,
                AuctionDate = treasuries[1].AuctionDate,
                IssueDate = treasuries[1].IssueDate,
                MaturityDate = treasuries[1].MaturityDate,
                SecurityTerm = treasuries[1].SecurityTerm,
                Type = treasuries[1].Type
            },
            new()
            {
                Id = 3,
                Confirmation = "BAZXX",
                Par = 2_000_400,
                Cusip = treasuries[2].Cusip,
                AuctionDate = treasuries[2].AuctionDate,
                IssueDate = treasuries[2].IssueDate,
                MaturityDate = treasuries[2].MaturityDate,
                SecurityTerm = treasuries[2].SecurityTerm,
                Type = treasuries[2].Type
            }
        ];

        modelBuilder.Entity<Investment>().HasData(investments);
    }
}