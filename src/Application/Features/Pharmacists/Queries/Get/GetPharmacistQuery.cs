using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Pharmacists.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Pharmacist;

namespace BigLion.CPA.Application.Features.Pharmacists.Queries.Get;

public class GetPharmacistQuery : IRequest<PharmacistViewModel>
{
    public int Id { get; set; }
}

public class GetPharmacistQueryHandler : IRequestHandler<GetPharmacistQuery, PharmacistViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetPharmacistQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PharmacistViewModel> Handle(GetPharmacistQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Pharmacists
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteFlag != true, cancellationToken)
            ?? throw new NotFoundException("Pharmacist", request.Id);

        return _mapper.Map<PharmacistViewModel>(entity);
    }
}
