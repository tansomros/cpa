using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.SubDistrict;

namespace BigLion.CPA.Application.Features.SubDistricts.Commands.Update;

public class UpdateSubDistrictCommand : IRequest<Unit>
{
    public string SubDistrictId { get; set; } = string.Empty;
    public string ProvinceId { get; set; } = string.Empty;
    public string DistrictId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}

public class UpdateSubDistrictCommandValidator : AbstractValidator<UpdateSubDistrictCommand>
{
    public UpdateSubDistrictCommandValidator()
    {
        RuleFor(x => x.ProvinceId).NotEmpty();
        RuleFor(x => x.DistrictId).NotEmpty();
        RuleFor(x => x.SubDistrictId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.NameEnglish).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
    }
}

public class UpdateSubDistrictCommandHandler : IRequestHandler<UpdateSubDistrictCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateSubDistrictCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSubDistrictCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.SubDistricts
            .FirstOrDefaultAsync(x => x.SubDistrictId == request.SubDistrictId, cancellationToken)
            ?? throw new NotFoundException("SubDistrict", request.SubDistrictId);

        entity.ProvinceId = request.ProvinceId;
        entity.DistrictId = request.DistrictId;
        entity.Name = request.Name;
        entity.NameEnglish = request.NameEnglish;
        entity.ZipCode = request.ZipCode;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
