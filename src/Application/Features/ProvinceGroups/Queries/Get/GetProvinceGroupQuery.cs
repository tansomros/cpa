using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.ProvinceGroups.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.ProvinceGroup;

namespace BigLion.CPA.Application.Features.ProvinceGroups.Queries.Get;

public class GetProvinceGroupQuery : IRequest<ProvinceGroupViewModel>
{
    public string Id { get; set; } = string.Empty;
}

public class GetProvinceGroupQueryHandler : IRequestHandler<GetProvinceGroupQuery, ProvinceGroupViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetProvinceGroupQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProvinceGroupViewModel> Handle(GetProvinceGroupQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProvinceGroups
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("ProvinceGroup", request.Id);

        return _mapper.Map<ProvinceGroupViewModel>(entity);
    }
}
