using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.PharmacyGroups.Commands.Delete;

public class DeletePharmacyGroupCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeletePharmacyGroupCommandHandler : IRequestHandler<DeletePharmacyGroupCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeletePharmacyGroupCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePharmacyGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PharmacyGroups
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteFlag != true, cancellationToken)
            ?? throw new NotFoundException("PharmacyGroup", request.Id);

        entity.IsActive = false;
        entity.DeleteFlag = true;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
