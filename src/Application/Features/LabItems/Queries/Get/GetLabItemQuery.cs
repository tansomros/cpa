using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.LabItems.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.LabItem;

namespace BigLion.CPA.Application.Features.LabItems.Queries.Get;

public class GetLabItemQuery : IRequest<LabItemViewModel>
{
    public int UID { get; set; }
}

public class GetLabItemQueryHandler : IRequestHandler<GetLabItemQuery, LabItemViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetLabItemQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LabItemViewModel> Handle(GetLabItemQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.LabItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("LabItem", request.UID);

        return _mapper.Map<LabItemViewModel>(entity);
    }
}
