using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.DrugMasters.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.DrugMaster;

namespace BigLion.CPA.Application.Features.DrugMasters.Queries.Get;

public class GetDrugMasterQuery : IRequest<DrugMasterViewModel>
{
    public int UID { get; set; }
}

public class GetDrugMasterQueryHandler : IRequestHandler<GetDrugMasterQuery, DrugMasterViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDrugMasterQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DrugMasterViewModel> Handle(GetDrugMasterQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.DrugMasters
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("DrugMaster", request.UID);

        return _mapper.Map<DrugMasterViewModel>(entity);
    }
}
