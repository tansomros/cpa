using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.ReferenceValues.ViewModels;

#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.Queries.Get;
[Obsolete("�� SmartEnum �ҡ Domain.Enums ᷹ � �� LookupRegistry.cs")]
public class GetReferenceValueByGroupQuery : IRequest<ReferenceValueListViewModel>
{    
    public required int ReferenceGroupId { get; set; }
}

[Obsolete("�� SmartEnum �ҡ Domain.Enums ᷹ � �� LookupRegistry.cs")]
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
