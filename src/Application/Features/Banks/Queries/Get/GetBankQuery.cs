using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Banks.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Bank;

namespace BigLion.CPA.Application.Features.Banks.Queries.Get;

public class GetBankQuery : IRequest<BankViewModel>
{
    public int Id { get; set; }
}

public class GetBankQueryHandler : IRequestHandler<GetBankQuery, BankViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetBankQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BankViewModel> Handle(GetBankQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Banks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Bank", request.Id);

        return _mapper.Map<BankViewModel>(entity);
    }
}
