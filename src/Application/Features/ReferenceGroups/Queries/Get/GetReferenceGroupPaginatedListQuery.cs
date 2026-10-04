using BigLion.CPA.Application.Common.Extensions;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.ReferenceGroups.ViewModels;

#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceGroups.Queries.Get;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class GetReferenceGroupPaginatedListQuery : IRequest<PaginatedList<ReferenceGroupViewModel>>
{
    public string? SearchTerm { get; set; }
    public required int Page { get; set; }
    public required int Length { get; set; }
}

[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class GetReferenceGroupPaginatedListQueryHandler
    : IRequestHandler<GetReferenceGroupPaginatedListQuery, PaginatedList<ReferenceGroupViewModel>>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetReferenceGroupPaginatedListQueryHandler(IMapper mapper, ICpaDatabaseContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedList<ReferenceGroupViewModel>> 
        Handle(GetReferenceGroupPaginatedListQuery request, CancellationToken cancellationToken)
    {
        var query = _context
            .ReferenceGroups
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Filter(request.SearchTerm);
        }

        query = query
            .OrderByDescending(d => d.LastModified)
            .ThenBy(d => d.CreatedOn);

        var count = await query.CountAsync(cancellationToken);
        var filtered = await query
            .AsNoTracking()
            .Skip((request.Page - 1) * request.Length)
            .Take(request.Length)
            .ToListAsync(cancellationToken);

        var viewModels = _mapper.Map<List<ReferenceGroupViewModel>>(filtered);
        return new PaginatedList<ReferenceGroupViewModel>(viewModels, count, request.Page, request.Length);
    }
}
