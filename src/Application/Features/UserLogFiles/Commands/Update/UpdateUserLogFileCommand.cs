using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.UserLogFile;

namespace BigLion.CPA.Application.Features.UserLogFiles.Commands.Update;

public class UpdateUserLogFileCommand : IRequest<Unit>
{
    public long LogID { get; set; }
    public int? UserID { get; set; }
    public DateTime? Work_Date { get; set; }
    public string? Act_Type { get; set; }
    public string? DB_Effective { get; set; }
    public string? Descrp { get; set; }
    public string? Remark { get; set; }
}

public class UpdateUserLogFileCommandValidator : AbstractValidator<UpdateUserLogFileCommand>
{
    public UpdateUserLogFileCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class UpdateUserLogFileCommandHandler : IRequestHandler<UpdateUserLogFileCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateUserLogFileCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserLogFileCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserLogFiles
            .FirstOrDefaultAsync(x => x.LogID == request.LogID, cancellationToken)
            ?? throw new NotFoundException("UserLogFile", request.LogID);

        entity.UserID = request.UserID;
        entity.Work_Date = request.Work_Date;
        entity.Act_Type = request.Act_Type;
        entity.DB_Effective = request.DB_Effective;
        entity.Descrp = request.Descrp;
        entity.Remark = request.Remark;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
