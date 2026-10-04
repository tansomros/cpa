using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Running;

namespace BigLion.CPA.Application.Features.Runnings.Commands.Update;

public class UpdateRunningCommand : IRequest<Unit>
{
    public string Code { get; set; } = string.Empty;
    public int RefCode { get; set; }
    public int LastRunning { get; set; }
}

public class UpdateRunningCommandValidator : AbstractValidator<UpdateRunningCommand>
{
    public UpdateRunningCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}

public class UpdateRunningCommandHandler : IRequestHandler<UpdateRunningCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateRunningCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateRunningCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Runnings
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("Running", request.Code);

        entity.RefCode = request.RefCode;
        entity.LastRunning = request.LastRunning;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
