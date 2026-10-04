using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Desease;

namespace BigLion.CPA.Application.Features.Deseases.Commands.Update;

public class UpdateDeseaseCommand : IRequest<Unit>
{
    public int UID { get; set; }
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

public class UpdateDeseaseCommandValidator : AbstractValidator<UpdateDeseaseCommand>
{
    public UpdateDeseaseCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateDeseaseCommandHandler : IRequestHandler<UpdateDeseaseCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateDeseaseCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDeseaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Deseases
            .FirstOrDefaultAsync(x => x.UID == request.UID, cancellationToken)
            ?? throw new NotFoundException("Desease", request.UID);

        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.ParentUID = request.ParentUID;
        entity.IsChapter = request.IsChapter;
        entity.ICD = request.ICD;
        entity.Sort = request.Sort;
        entity.StatusFlag = request.StatusFlag;
        entity.isICD = request.isICD;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
