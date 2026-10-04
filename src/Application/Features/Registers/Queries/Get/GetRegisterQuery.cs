using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Registers.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Register;

namespace BigLion.CPA.Application.Features.Registers.Queries.Get;

public class GetRegisterQuery : IRequest<RegisterViewModel>
{
    public int UID { get; set; }
}

public class GetRegisterQueryHandler : IRequestHandler<GetRegisterQuery, RegisterViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetRegisterQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RegisterViewModel> Handle(GetRegisterQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Registers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("Register", request.UID);

        return _mapper.Map<RegisterViewModel>(entity);
    }
}
