using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.SubDistricts.Commands.Delete;

public class DeleteSubDistrictCommand : IRequest<Unit>
{
    public string SubDistrictId { get; set; } = string.Empty;
}

public class DeleteSubDistrictCommandHandler : IRequestHandler<DeleteSubDistrictCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteSubDistrictCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteSubDistrictCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.SubDistricts
            .FirstOrDefaultAsync(x => x.SubDistrictId == request.SubDistrictId, cancellationToken)
            ?? throw new NotFoundException("SubDistrict", request.SubDistrictId);

        _context.SubDistricts.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
