using Microsoft.EntityFrameworkCore;
using Pip.Model;

namespace Pip.DataAccess;

public class PipDbContext(DbContextOptions<PipDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Treasury>().HasData([
            new Treasury
            {
                Cusip = "912797GL5",
                IssueDate = new DateOnly(2024, 7, 25),
                MaturityDate = new DateOnly(2024, 9, 5),
                SecurityType = "Bill", SecurityTerm = "42-Day"
            },
            new Treasury
            {
                Cusip = "912797KX4",
                IssueDate = new DateOnly(2024, 6, 18),
                MaturityDate = new DateOnly(2024, 8, 13),
                SecurityType = "Bill", SecurityTerm = "8-Week"
            },
            new Treasury
            {
                Cusip = "912797GK7",
                IssueDate = new DateOnly(2024, 5, 9),
                MaturityDate = new DateOnly(2024, 8, 8),
                SecurityType = "Bill", SecurityTerm = "13-Week"
            }
        ]);
    }
}
