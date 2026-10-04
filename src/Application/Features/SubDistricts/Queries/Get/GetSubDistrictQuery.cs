using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.SubDistricts.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.SubDistrict;

namespace BigLion.CPA.Application.Features.SubDistricts.Queries.Get;

public class GetSubDistrictQuery : IRequest<SubDistrictViewModel>
{
    public string SubDistrictId { get; set; } = string.Empty;
}

public class GetSubDistrictQueryHandler : IRequestHandler<GetSubDistrictQuery, SubDistrictViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetSubDistrictQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SubDistrictViewModel> Handle(GetSubDistrictQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.SubDistricts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SubDistrictId == request.SubDistrictId, cancellationToken)
            ?? throw new NotFoundException("SubDistrict", request.SubDistrictId);

        return _mapper.Map<SubDistrictViewModel>(entity);
    }
}
