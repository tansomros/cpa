using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Features.ReferenceValues.Commands.Update;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class UpdateReferenceValueCommand : IRequest<Unit>
{
    public required int Id { get; set; }
    public required string ValueCode { get; set; }
    public required string Descriptions { get; set; }
    public int ReferenceGroupId { get; set; }
    public int Sort { get; set; }
}

[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class UpdateReferenceValueCommandHandler : IRequestHandler<UpdateReferenceValueCommand, Unit>
{
    private readonly ICpaDatabaseContext _checkupContext;
    public UpdateReferenceValueCommandHandler(ICpaDatabaseContext checkupContext)
    {
        _checkupContext = checkupContext;
    }

    public async Task<Unit> Handle(UpdateReferenceValueCommand request, CancellationToken cancellationToken)
    {
        var group = await _checkupContext.ReferenceValues
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (group == null)
        {
            throw new NotFoundException(nameof(ReferenceValue), request.Id);
        }

        group.ValueCode = request.ValueCode;
        group.Descriptions = request.Descriptions;
        group.ReferenceGroupId = request.ReferenceGroupId;
        group.Sort = request.Sort;

        _checkupContext.ReferenceValues.Update(group);
        await _checkupContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
