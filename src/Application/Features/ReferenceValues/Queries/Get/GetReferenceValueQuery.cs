using BigLion.CPA.Application.Features.ReferenceValues.ViewModels;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Domain.Entities;


#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.Queries.Get;
[Obsolete("�� SmartEnum �ҡ Domain.Enums ᷹ � �� LookupRegistry.cs")]
public class GetReferenceValueQuery : IRequest<ReferenceValueViewModel>
{
    public required int Id { get; set; }
}

[Obsolete("�� SmartEnum �ҡ Domain.Enums ᷹ � �� LookupRegistry.cs")]
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
