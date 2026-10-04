using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.DrugProblemItems.Commands.Delete;

public class DeleteDrugProblemItemCommand : IRequest<Unit>
{
    public string Code { get; set; } = string.Empty;
}

public class DeleteDrugProblemItemCommandHandler : IRequestHandler<DeleteDrugProblemItemCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteDrugProblemItemCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDrugProblemItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DrugProblemItems
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("DrugProblemItem", request.Code);

        _context.DrugProblemItems.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
