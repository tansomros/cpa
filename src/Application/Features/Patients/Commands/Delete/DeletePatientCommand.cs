using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;

namespace Cpa.Application.Features.Patients.Commands.Delete;

public record DeletePatientCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
public class DeleteCommandHandler : IRequestHandler<DeletePatientCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {

        var entity = await _context.Patients
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Patients), request.Id);
        }

        _context.Patients.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}
