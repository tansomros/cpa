using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.RunningConfigs.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.RunningConfig;

namespace BigLion.CPA.Application.Features.RunningConfigs.Queries.Get;

public class GetRunningConfigQuery : IRequest<RunningConfigViewModel>
{
    public string Code { get; set; } = string.Empty;
}

public class GetRunningConfigQueryHandler : IRequestHandler<GetRunningConfigQuery, RunningConfigViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetRunningConfigQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RunningConfigViewModel> Handle(GetRunningConfigQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.RunningConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("RunningConfig", request.Code);

        return _mapper.Map<RunningConfigViewModel>(entity);
    }
}
