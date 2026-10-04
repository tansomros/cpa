using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.RunningConfig;

namespace BigLion.CPA.Application.Features.RunningConfigs.Commands.Create;

public class CreateRunningConfigCommand : IRequest<string>
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCode { get; set; }
    public bool IsRef { get; set; }
    public int DigitCount { get; set; }
    public string? TemplateCode { get; set; }
}

public class CreateRunningConfigCommandValidator : AbstractValidator<CreateRunningConfigCommand>
{
    public CreateRunningConfigCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}

public class CreateRunningConfigCommandHandler : IRequestHandler<CreateRunningConfigCommand, string>
{
    private readonly ICpaDatabaseContext _context;

    public CreateRunningConfigCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateRunningConfigCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(request.Code, request.Description, request.IsCode, request.IsRef, request.DigitCount);
        entity.Code = request.Code;
        entity.Description = request.Description;
        entity.IsCode = request.IsCode;
        entity.IsRef = request.IsRef;
        entity.DigitCount = request.DigitCount;
        entity.TemplateCode = request.TemplateCode;
        await _context.RunningConfigs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Code;
    }
}
