using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Bank;

namespace BigLion.CPA.Application.Features.Banks.Commands.Create;

public class CreateBankCommand : IRequest<int>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class CreateBankCommandValidator : AbstractValidator<CreateBankCommand>
{
    public CreateBankCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateBankCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateBankCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(0, request.Code, request.Name);
        entity.Code = request.Code;
        entity.Name = request.Name;
        await _context.Banks.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
