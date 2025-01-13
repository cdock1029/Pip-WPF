using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Td.Data;

public class TreasuryService : ITreasuryService
{
    private static readonly Func<TreasuryDirectorContext, IEnumerable<Investment>> CompiledGetInvestments
        = EF.CompileQuery((TreasuryDirectorContext context) => context.Investments);

    private readonly IMemoryCache _cache;

    private readonly TreasuryDirectorContext _dbContext;
    private readonly HttpClient _httpClient;

    public TreasuryService(HttpClient httpClient, IMemoryCache cache, TreasuryDirectorContext dbContext)
    {
        httpClient.BaseAddress = new Uri("https://www.treasurydirect.gov/TA_WS/");
        _httpClient = httpClient;
        _cache = cache;
        _dbContext = dbContext;
    }

    public async Task<List<Investment>?> GetInvestmentsAsync()
    {
        return await _dbContext.Investments.ToListAsync();
    }

    public List<Investment> GetInvestments()
    {
        return CompiledGetInvestments(_dbContext).ToList();
    }

    public async Task<Treasury?> GetTreasuryAsync(string cusip, DateOnly issueDate)
    {
        var key = (cusip, issueDate);
        var treasury = await _cache.GetOrCreateAsync(key, e =>
        {
            e.SetOptions(new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            });

            var issueDateString = issueDate.ToString("MM'/'dd'/'yyyy");
            return _httpClient.GetFromJsonAsync<Treasury>($"securities/{cusip}/{issueDateString}?format=json");
        });
        return treasury;
    }

    public void Add(Investment investment)
    {
        _dbContext.Add(investment);
        _dbContext.SaveChanges();
    }

    public async Task AddInvestmentAsync(Investment investment)
    {
        _dbContext.Add(investment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Treasury>?> SearchTreasuriesAsync(string cusip)
    {
        var treasuries =
            await _httpClient.GetFromJsonAsync<List<Treasury>>($"securities/search/?format=json&cusip={cusip}");
        return treasuries;
    }

    public IAsyncEnumerable<Treasury?> GetUpcomingAsync(CancellationToken ct)
    {
        return _httpClient.GetFromJsonAsAsyncEnumerable<Treasury>("securities/upcoming/?format=json", ct);
    }

    public IAsyncEnumerable<Treasury?> GetRecentAsync(CancellationToken ct)
    {
        return _httpClient.GetFromJsonAsAsyncEnumerable<Treasury>(
            "securities/auctioned?format=json&limitByTerm=true&days=720", ct);
    }

    public void Delete(Investment investment)
    {
        _dbContext.Remove(investment);
        _dbContext.SaveChanges();
    }

    public async Task DeleteInvestmentAsync(Investment investment)
    {
        _dbContext.Remove(investment);
        await _dbContext.SaveChangesAsync();
    }

    public void Update(Investment investment)
    {
        var lookup = _dbContext.Find<Investment>(investment.Id);
        if (lookup is null) return;

        _dbContext.Entry(lookup).CurrentValues.SetValues(investment);
        _dbContext.SaveChanges();
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    public Task<IEnumerable<Treasury>?> GetRecentAsyncSimple()
    {
        return _httpClient.GetFromJsonAsync<IEnumerable<Treasury>>(
            "securities/auctioned?format=json&limitByTerm=true&days=720");
    }


    public Task<IEnumerable<Treasury>?> GetUpcomingAsyncSimple()
    {
        return _httpClient.GetFromJsonAsync<IEnumerable<Treasury>>("securities/upcoming/?format=json");
    }
}