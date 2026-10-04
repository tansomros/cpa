using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.PharmacyTypes.Commands.Delete;

public class DeletePharmacyTypeCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeletePharmacyTypeCommandHandler : IRequestHandler<DeletePharmacyTypeCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeletePharmacyTypeCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePharmacyTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PharmacyTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteFlag != true, cancellationToken)
            ?? throw new NotFoundException("PharmacyType", request.Id);

        entity.IsActive = false;
        entity.DeleteFlag = true;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
