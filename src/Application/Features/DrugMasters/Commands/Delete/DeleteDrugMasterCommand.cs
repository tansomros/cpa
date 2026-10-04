using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.DrugMasters.Commands.Delete;

public class DeleteDrugMasterCommand : IRequest<Unit>
{
    public int UID { get; set; }
}

public class DeleteDrugMasterCommandHandler : IRequestHandler<DeleteDrugMasterCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteDrugMasterCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDrugMasterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DrugMasters
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("DrugMaster", request.UID);

        _context.DrugMasters.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
