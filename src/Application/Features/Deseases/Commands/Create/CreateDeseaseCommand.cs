using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Desease;

namespace BigLion.CPA.Application.Features.Deseases.Commands.Create;

public class CreateDeseaseCommand : IRequest<int>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? ParentUID { get; set; }
    public string? IsChapter { get; set; }
    public string? ICD { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
    public string? isICD { get; set; }
}

public class CreateDeseaseCommandValidator : AbstractValidator<CreateDeseaseCommand>
{
    public CreateDeseaseCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateDeseaseCommandHandler : IRequestHandler<CreateDeseaseCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateDeseaseCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDeseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.ParentUID = request.ParentUID;
        entity.IsChapter = request.IsChapter;
        entity.ICD = request.ICD;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        entity.isICD = request.isICD;
        await _context.Deseases.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
