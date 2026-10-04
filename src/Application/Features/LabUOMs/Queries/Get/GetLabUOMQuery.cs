using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.LabUOMs.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.LabUOM;

namespace BigLion.CPA.Application.Features.LabUOMs.Queries.Get;

public class GetLabUOMQuery : IRequest<LabUOMViewModel>
{
    public int UID { get; set; }
}

public class GetLabUOMQueryHandler : IRequestHandler<GetLabUOMQuery, LabUOMViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetLabUOMQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LabUOMViewModel> Handle(GetLabUOMQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.LabUOMs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("LabUOM", request.UID);

        return _mapper.Map<LabUOMViewModel>(entity);
    }
}
