using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.UserLogFile;

namespace BigLion.CPA.Application.Features.UserLogFiles.Commands.Create;

public class CreateUserLogFileCommand : IRequest<long>
{
    public int? UserID { get; set; }
    public DateTime? Work_Date { get; set; }
    public string? Act_Type { get; set; }
    public string? DB_Effective { get; set; }
    public string? Descrp { get; set; }
    public string? Remark { get; set; }
}

public class CreateUserLogFileCommandValidator : AbstractValidator<CreateUserLogFileCommand>
{
    public CreateUserLogFileCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateUserLogFileCommandHandler : IRequestHandler<CreateUserLogFileCommand, long>
{
    private readonly ICpaDatabaseContext _context;

    public CreateUserLogFileCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateUserLogFileCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.UserID = request.UserID;
        entity.Work_Date = request.Work_Date;
        entity.Act_Type = request.Act_Type;
        entity.DB_Effective = request.DB_Effective;
        entity.Descrp = request.Descrp;
        entity.Remark = request.Remark;
        await _context.UserLogFiles.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.LogID;
    }
}
