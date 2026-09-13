using Cpa.Application.Features.ReferenceValues.ViewModels;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;
using Cpa.Domain.Entities;


#pragma warning disable CS0618
namespace Cpa.Application.Features.ReferenceValues.Queries.Get;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class GetReferenceValueQuery : IRequest<ReferenceValueViewModel>
{
    public required int Id { get; set; }
}

[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class GetReferenceValueQueryHandler : IRequestHandler<GetReferenceValueQuery, ReferenceValueViewModel>
{
    private readonly IMapper _mapper;
    private readonly ICpaDatabaseContext _checkupContext;

    public GetReferenceValueQueryHandler(ICpaDatabaseContext checkupContext, IMapper mapper)
    {
        _checkupContext = checkupContext;
        _mapper = mapper;
    }

    public async Task<ReferenceValueViewModel> Handle(GetReferenceValueQuery request, CancellationToken cancellationToken)
    {
        var template = await _checkupContext.ReferenceValues
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (template == null)
        {
            throw new NotFoundException(nameof(ReferenceValue), request.Id);
        }

        return _mapper.Map<ReferenceValueViewModel>(template);
    }
}
