using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.RunningConfig;

namespace BigLion.CPA.Application.Features.RunningConfigs.Commands.Update;

public class UpdateRunningConfigCommand : IRequest<Unit>
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCode { get; set; }
    public bool IsRef { get; set; }
    public int DigitCount { get; set; }
    public string? TemplateCode { get; set; }
}

public class UpdateRunningConfigCommandValidator : AbstractValidator<UpdateRunningConfigCommand>
{
    public UpdateRunningConfigCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}

public class UpdateRunningConfigCommandHandler : IRequestHandler<UpdateRunningConfigCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateRunningConfigCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateRunningConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.RunningConfigs
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("RunningConfig", request.Code);

        entity.Description = request.Description;
        entity.IsCode = request.IsCode;
        entity.IsRef = request.IsRef;
        entity.DigitCount = request.DigitCount;
        entity.TemplateCode = request.TemplateCode;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
