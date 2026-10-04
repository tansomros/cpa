using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Runnings.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.Running;

namespace BigLion.CPA.Application.Features.Runnings.Queries.Get;

public class GetRunningQuery : IRequest<RunningViewModel>
{
    public string Code { get; set; } = string.Empty;
}

public class GetRunningQueryHandler : IRequestHandler<GetRunningQuery, RunningViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetRunningQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RunningViewModel> Handle(GetRunningQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Runnings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("Running", request.Code);

        return _mapper.Map<RunningViewModel>(entity);
    }
}
