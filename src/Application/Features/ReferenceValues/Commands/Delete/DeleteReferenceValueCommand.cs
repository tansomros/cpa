using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Features.ReferenceValues.Commands.Delete;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class DeleteReferenceValueCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}

[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class DeleteReferenceValueCommandHandler : IRequestHandler<DeleteReferenceValueCommand, Unit>
{
    private readonly ICpaDatabaseContext _checkupContext;
    public DeleteReferenceValueCommandHandler(ICpaDatabaseContext checkupContext)
    {
        _checkupContext = checkupContext;
    }

    public async Task<Unit> Handle(DeleteReferenceValueCommand request, CancellationToken cancellationToken)
    {
        var template = await _checkupContext.ReferenceValues
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (template == null)
        {
            throw new NotFoundException(nameof(ReferenceValue), request.Id);
        }

        _checkupContext.ReferenceValues.Remove(template);
        await _checkupContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
