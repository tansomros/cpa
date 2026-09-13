using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Features.ReferenceGroups.Commands.Delete;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class DeleteReferenceGroupCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}

[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class DeleteReferenceGroupCommandHandler : IRequestHandler<DeleteReferenceGroupCommand, Unit>
{
    private readonly ICpaDatabaseContext _checkupContext;
    public DeleteReferenceGroupCommandHandler(ICpaDatabaseContext checkupContext)
    {
        _checkupContext = checkupContext;
    }

    public async Task<Unit> Handle(DeleteReferenceGroupCommand request, CancellationToken cancellationToken)
    {
        var template = await _checkupContext.ReferenceGroups
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (template == null)
        {
            throw new NotFoundException(nameof(ReferenceGroup), request.Id);
        }

        _checkupContext.ReferenceGroups.Remove(template);
        await _checkupContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
