using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Bank;

namespace BigLion.CPA.Application.Features.Banks.Commands.Update;

public class UpdateBankCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class UpdateBankCommandValidator : AbstractValidator<UpdateBankCommand>
{
    public UpdateBankCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateBankCommandHandler : IRequestHandler<UpdateBankCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateBankCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Banks
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Bank", request.Id);

        entity.Code = request.Code;
        entity.Name = request.Name;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
