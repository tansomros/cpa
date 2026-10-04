using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.ServiceTypes.Commands.Delete;

public class DeleteServiceTypeCommand : IRequest<Unit>
{
    public string ServiceTypeID { get; set; } = string.Empty;
}

public class DeleteServiceTypeCommandHandler : IRequestHandler<DeleteServiceTypeCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteServiceTypeCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteServiceTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ServiceTypes
            .FirstOrDefaultAsync(x => x.ServiceTypeID == request.ServiceTypeID, cancellationToken)
            ?? throw new NotFoundException("ServiceType", request.ServiceTypeID);

        _context.ServiceTypes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
