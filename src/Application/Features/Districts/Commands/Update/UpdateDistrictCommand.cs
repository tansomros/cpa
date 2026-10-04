using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.District;

namespace BigLion.CPA.Application.Features.Districts.Commands.Update;

public class UpdateDistrictCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string ProvinceId { get; set; } = string.Empty;
}

public class UpdateDistrictCommandValidator : AbstractValidator<UpdateDistrictCommand>
{
    public UpdateDistrictCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.NameEnglish).NotEmpty();
        RuleFor(x => x.ProvinceId).NotEmpty();
    }
}

public class UpdateDistrictCommandHandler : IRequestHandler<UpdateDistrictCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateDistrictCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateDistrictCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Districts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("District", request.Id);

        entity.Name = request.Name;
        entity.NameEnglish = request.NameEnglish;
        entity.ProvinceId = request.ProvinceId;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
