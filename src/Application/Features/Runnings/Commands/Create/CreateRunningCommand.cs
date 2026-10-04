using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Running;

namespace BigLion.CPA.Application.Features.Runnings.Commands.Create;

public class CreateRunningCommand : IRequest<string>
{
    public string Code { get; set; } = string.Empty;
    public int RefCode { get; set; }
    public int LastRunning { get; set; }
}

public class CreateRunningCommandValidator : AbstractValidator<CreateRunningCommand>
{
    public CreateRunningCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}

public class CreateRunningCommandHandler : IRequestHandler<CreateRunningCommand, string>
{
    private readonly ICpaDatabaseContext _context;

    public CreateRunningCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateRunningCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(request.Code, request.RefCode, request.LastRunning);
        entity.Code = request.Code;
        entity.RefCode = request.RefCode;
        entity.LastRunning = request.LastRunning;
        await _context.Runnings.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Code;
    }
}
