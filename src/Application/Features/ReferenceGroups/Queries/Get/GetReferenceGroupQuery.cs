using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;
using Cpa.Application.Features.ReferenceGroups.ViewModels;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Features.ReferenceGroups.Queries.Get;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class GetReferenceGroupQuery : IRequest<ReferenceGroupViewModel>
{
    public required int Id { get; set; }
}

[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class GetReferenceGroupQueryHandler : IRequestHandler<GetReferenceGroupQuery, ReferenceGroupViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _checkupContext;

    public GetReferenceGroupQueryHandler(ICpaDatabaseContext checkupContext, IMapper mapper)
    {
        _checkupContext = checkupContext;
        _mapper = mapper;
    }

    public async Task<ReferenceGroupViewModel> Handle(GetReferenceGroupQuery request, CancellationToken cancellationToken)
    {
        var template = await _checkupContext.ReferenceGroups
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (template == null)
        {
            throw new NotFoundException(nameof(ReferenceGroup), request.Id);
        }

        return _mapper.Map<ReferenceGroupViewModel>(template);
    }
}
