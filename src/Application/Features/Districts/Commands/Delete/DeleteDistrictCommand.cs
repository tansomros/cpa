using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Districts.Commands.Delete;

public class DeleteDistrictCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}

public class DeleteDistrictCommandHandler : IRequestHandler<DeleteDistrictCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteDistrictCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDistrictCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Districts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("District", request.Id);

        _context.Districts.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
