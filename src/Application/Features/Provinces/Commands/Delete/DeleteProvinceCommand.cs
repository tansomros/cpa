using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Provinces.Commands.Delete;

public class DeleteProvinceCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}

public class DeleteProvinceCommandHandler : IRequestHandler<DeleteProvinceCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public DeleteProvinceCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProvinceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Provinces
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Province", request.Id);

        _context.Provinces.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
