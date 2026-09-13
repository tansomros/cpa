using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Kondongpu.Application.Common.Interfaces;

namespace Kondongpu.Application.Services;
public class CheckupItemCacheService : ICheckupItemCacheService
{
    private readonly IMemoryCache _cache;
    private readonly KondongpuDatabaseContext _context;

    private const string CacheKey = "CheckupItem_OrderCodes";

    public CheckupItemCacheService(
        IMemoryCache cache,
        KondongpuDatabaseContext context)
    {
        _cache = cache;
        _context = context;
    }

    public async Task<Dictionary<string, HashSet<string>>> GetAllOrderCodesAsync()
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6);

            var items = await _context.CheckupItems
                .AsNoTracking()
                .Select(x => new
                {
                    x.Code,
                    x.LabItemCode
                })
                .ToListAsync();

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
#pragma warning disable CS8621 // Nullability of reference types in return type doesn't match the target delegate (possibly because of nullability attributes).
            return items
                .GroupBy(x => x.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.LabItemCode).ToHashSet());
#pragma warning restore CS8621 // Nullability of reference types in return type doesn't match the target delegate (possibly because of nullability attributes).
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        }) ?? new Dictionary<string, HashSet<string>>();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }
}
