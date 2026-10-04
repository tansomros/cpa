using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Districts.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.District;

namespace BigLion.CPA.Application.Features.Districts.Queries.Get;

public class GetDistrictQuery : IRequest<DistrictViewModel>
{
    public string Id { get; set; } = string.Empty;
}

public class GetDistrictQueryHandler : IRequestHandler<GetDistrictQuery, DistrictViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDistrictQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DistrictViewModel> Handle(GetDistrictQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Districts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("District", request.Id);

        return _mapper.Map<DistrictViewModel>(entity);
    }
}
