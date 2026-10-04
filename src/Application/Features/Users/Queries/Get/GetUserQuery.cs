using BigLion.CPA.Domain.Entities;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.Users.ViewModel;

namespace BigLion.CPA.Application.Features.Users.Queries.Get;

public record GetUserQuery : IRequest<UserViewModel>
{
    public required int Id { get; set; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserViewModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {

        var specialTest = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.Id);


        var model = _mapper.Map<UserViewModel>(specialTest);
        return model;
    }
}
