using Cpa.Application.Common.Interfaces;
using Cpa.Application.Features.ReferenceValues.ViewModels;

#pragma warning disable CS0618
namespace Cpa.Application.Features.ReferenceValues.Queries.Get;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class GetReferenceValueByGroupQuery : IRequest<ReferenceValueListViewModel>
{    
    public required int ReferenceGroupId { get; set; }
}

[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class GetReferenceValueByGroupQueryHandler : IRequestHandler<GetReferenceValueByGroupQuery, ReferenceValueListViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _context;

    public GetReferenceValueByGroupQueryHandler(IMapper mapper, ICpaDatabaseContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ReferenceValueListViewModel> Handle(GetReferenceValueByGroupQuery request, CancellationToken cancellationToken)
    {
        var referenceValues = await _context
            .ReferenceValues.AsNoTracking().Include(c => c.ReferenceGroup)
            .Where(c => c.ReferenceGroupId == request.ReferenceGroupId)
            .ToListAsync(cancellationToken);

        var referenceValueList = _mapper.Map<List<ReferenceValueViewModel>>(referenceValues);

        return new ReferenceValueListViewModel()
        {
            ReferenceValues = referenceValueList
        };
    }
}
