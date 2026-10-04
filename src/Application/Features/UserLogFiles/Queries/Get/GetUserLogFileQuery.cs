using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.UserLogFiles.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.UserLogFile;

namespace BigLion.CPA.Application.Features.UserLogFiles.Queries.Get;

public class GetUserLogFileQuery : IRequest<UserLogFileViewModel>
{
    public long LogID { get; set; }
}

public class GetUserLogFileQueryHandler : IRequestHandler<GetUserLogFileQuery, UserLogFileViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetUserLogFileQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserLogFileViewModel> Handle(GetUserLogFileQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserLogFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LogID == request.LogID, cancellationToken)
            ?? throw new NotFoundException("UserLogFile", request.LogID);

        return _mapper.Map<UserLogFileViewModel>(entity);
    }
}
