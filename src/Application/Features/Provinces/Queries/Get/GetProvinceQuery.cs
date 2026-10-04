using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Provinces.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Province;

namespace BigLion.CPA.Application.Features.Provinces.Queries.Get;

public class GetProvinceQuery : IRequest<ProvinceViewModel>
{
    public string Id { get; set; } = string.Empty;
}

public class GetProvinceQueryHandler : IRequestHandler<GetProvinceQuery, ProvinceViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetProvinceQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProvinceViewModel> Handle(GetProvinceQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Provinces
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Province", request.Id);

        return _mapper.Map<ProvinceViewModel>(entity);
    }
}
