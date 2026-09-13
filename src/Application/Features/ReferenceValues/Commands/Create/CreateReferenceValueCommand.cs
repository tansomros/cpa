using Cpa.Application.Common.Interfaces;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Features.ReferenceValues.Commands.Create;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class CreateReferenceValueCommand : IRequest<int>
{
    public required string ValueCode { get; set; }
    public required string Descriptions { get; set; }
    public int ReferenceGroupId { get; set; }
    public int Sort { get; set; }
}

[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class CreateReferenceValueCommandHandler : IRequestHandler<CreateReferenceValueCommand, int>
{
    private readonly ICpaDatabaseContext _checkupContext;

    public CreateReferenceValueCommandHandler(ICpaDatabaseContext checkupDatabaseContext)
    {
        _checkupContext = checkupDatabaseContext;
    }

    public async Task<int> Handle(CreateReferenceValueCommand request, CancellationToken cancellationToken)
    {
        var entity = new ReferenceValue(request.ValueCode, request.Descriptions, request.ReferenceGroupId, request.Sort);
        _checkupContext.ReferenceValues.Add(entity);
        await _checkupContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
