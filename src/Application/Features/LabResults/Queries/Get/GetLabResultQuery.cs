using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.LabResults.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.LabResult;

namespace BigLion.CPA.Application.Features.LabResults.Queries.Get;

public class GetLabResultQuery : IRequest<LabResultViewModel>
{
    public int UID { get; set; }
}

public class GetLabResultQueryHandler : IRequestHandler<GetLabResultQuery, LabResultViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetLabResultQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LabResultViewModel> Handle(GetLabResultQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.LabResults
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("LabResult", request.UID);

        return _mapper.Map<LabResultViewModel>(entity);
    }
}
