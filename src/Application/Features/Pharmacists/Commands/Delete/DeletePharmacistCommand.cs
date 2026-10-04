using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Pharmacists.Commands.Delete;

public class DeletePharmacistCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeletePharmacistCommandHandler : IRequestHandler<DeletePharmacistCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeletePharmacistCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePharmacistCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Pharmacists
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteFlag != true, cancellationToken)
            ?? throw new NotFoundException("Pharmacist", request.Id);

        entity.IsActive = false;
        entity.DeleteFlag = true;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
