using BigLion.CPA.Application.Features.ReferenceValues.ViewModels;
using BigLion.CPA.Application.Common.Extensions;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Models;

#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.Queries.Get;
[Obsolete("�� SmartEnum �ҡ Domain.Enums ᷹ � �� LookupRegistry.cs")]
public class GetReferenceValuePaginatedListQuery : IRequest<PaginatedList<ReferenceValueViewModel>>
{
    public string? SearchTerm { get; set; }
    public required int Page { get; set; }
    public required int Length { get; set; }
}

[Obsolete("�� SmartEnum �ҡ Domain.Enums ᷹ � �� LookupRegistry.cs")]
public class GetReferenceValuePaginatedListQueryHandler
    : IRequestHandler<GetReferenceValuePaginatedListQuery, PaginatedList<ReferenceValueViewModel>>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetReferenceValuePaginatedListQueryHandler(IMapper mapper, ICpaDatabaseContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedList<ReferenceValueViewModel>> 
        Handle(GetReferenceValuePaginatedListQuery request, CancellationToken cancellationToken)
    {
        var query = _context
            .ReferenceValues
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

        var viewModels = _mapper.Map<List<ReferenceValueViewModel>>(filtered);
        return new PaginatedList<ReferenceValueViewModel>(viewModels, count, request.Page, request.Length);
    }
}
