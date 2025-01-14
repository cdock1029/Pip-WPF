using Microsoft.EntityFrameworkCore;
using Pip.Model;

namespace Pip.DataAccess;

public class PipDbContext : DbContext
{
	public DbSet<Investment> Investments { get; init; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite("Data Source=pip.db");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);


		List<Treasury> treasuries =
		[
			new()
			{
				Cusip = "912797GL5",
				IssueDate = new DateOnly(2024, 7, 25),
				MaturityDate = new DateOnly(2024, 9, 5),
				SecurityType = TreasurySecurityType.Bill, SecurityTerm = "42-Day", Type = TreasuryType.Bill
			},
			new()
			{
				Cusip = "912797KX4",
				IssueDate = new DateOnly(2024, 6, 18),
				MaturityDate = new DateOnly(2024, 8, 13),
				SecurityType = TreasurySecurityType.Bill, SecurityTerm = "8-Week", Type = TreasuryType.Bill
			},
			new()
			{
				Cusip = "912797GK7",
				IssueDate = new DateOnly(2024, 5, 9),
				MaturityDate = new DateOnly(2024, 8, 8),
				SecurityType = TreasurySecurityType.Bill, SecurityTerm = "13-Week", Type = TreasuryType.Bill
			},
			new()
			{
				Cusip = "912797GL5",
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
				Confirmation = "FOO",
				Par = 100_000,
				Cusip = treasuries[0].Cusip,
				IssueDate = treasuries[0].IssueDate!.Value,
				MaturityDate = treasuries[0].MaturityDate,
				SecurityTerm = treasuries[0].SecurityTerm,
				Type = treasuries[0].Type
			},
			new()
			{
				Id = 2,

				Confirmation = "BAR",
				Par = 55_000,
				Cusip = treasuries[1].Cusip,
				IssueDate = treasuries[1].IssueDate!.Value,
				MaturityDate = treasuries[1].MaturityDate,
				SecurityTerm = treasuries[1].SecurityTerm,
				Type = treasuries[1].Type
			},
			new()
			{
				Id = 3,
				Confirmation = "BAZ",
				Par = 2000_400,
				Cusip = treasuries[2].Cusip,
				IssueDate = treasuries[2].IssueDate!.Value,
				MaturityDate = treasuries[2].MaturityDate,
				SecurityTerm = treasuries[2].SecurityTerm,
				Type = treasuries[2].Type
			}
		];

		modelBuilder.Entity<Investment>().HasData(investments);
	}
}