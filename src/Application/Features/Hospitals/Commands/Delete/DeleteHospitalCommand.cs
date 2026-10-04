using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Hospitals.Commands.Delete;

public class DeleteHospitalCommand : IRequest<Unit>
{
    public int HospitalUID { get; set; }
}

public class DeleteHospitalCommandHandler : IRequestHandler<DeleteHospitalCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteHospitalCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteHospitalCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Hospitals
            .FirstOrDefaultAsync(x => x.HospitalUID == request.HospitalUID, cancellationToken)
            ?? throw new NotFoundException("Hospital", request.HospitalUID);

        _context.Hospitals.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
