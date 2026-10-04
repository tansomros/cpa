using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.DrugProblemGroups.Commands.Delete;

public class DeleteDrugProblemGroupCommand : IRequest<Unit>
{
    public string Code { get; set; } = string.Empty;
}

public class DeleteDrugProblemGroupCommandHandler : IRequestHandler<DeleteDrugProblemGroupCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteDrugProblemGroupCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDrugProblemGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DrugProblemGroups
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("DrugProblemGroup", request.Code);

        _context.DrugProblemGroups.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
