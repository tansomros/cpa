using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Domain.Entities;

#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceGroups.Commands.Create;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class CreateReferenceGroupCommand : IRequest<int>
{
    public required string Code { get; set; }
    public required string Descriptions { get; set; }
    public int Sort { get; set; }
}

[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class CreateReferenceGroupCommandHandler : IRequestHandler<CreateReferenceGroupCommand,int>
{
    private readonly ICpaDatabaseContext _checkupContext;
    public CreateReferenceGroupCommandHandler(ICpaDatabaseContext checkupContext)
    {
        _checkupContext = checkupContext;
    }

    public async Task<int> Handle(CreateReferenceGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new ReferenceGroup(request.Code, request.Descriptions, request.Sort);
        _checkupContext.ReferenceGroups.Add(entity);
        await _checkupContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
