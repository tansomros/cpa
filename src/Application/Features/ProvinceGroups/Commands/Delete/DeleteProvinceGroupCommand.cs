using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.ProvinceGroups.Commands.Delete;

public class DeleteProvinceGroupCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}

public class DeleteProvinceGroupCommandHandler : IRequestHandler<DeleteProvinceGroupCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteProvinceGroupCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProvinceGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProvinceGroups
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("ProvinceGroup", request.Id);

        _context.ProvinceGroups.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
