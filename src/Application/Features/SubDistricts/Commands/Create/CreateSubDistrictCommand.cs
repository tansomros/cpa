using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.SubDistrict;

namespace BigLion.CPA.Application.Features.SubDistricts.Commands.Create;

public class CreateSubDistrictCommand : IRequest<string>
{
    public string ProvinceId { get; set; } = string.Empty;
    public string DistrictId { get; set; } = string.Empty;
    public string SubDistrictId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}

public class CreateSubDistrictCommandValidator : AbstractValidator<CreateSubDistrictCommand>
{
    public CreateSubDistrictCommandValidator()
    {
        RuleFor(x => x.ProvinceId).NotEmpty();
        RuleFor(x => x.DistrictId).NotEmpty();
        RuleFor(x => x.SubDistrictId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.NameEnglish).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
    }
}

public class CreateSubDistrictCommandHandler : IRequestHandler<CreateSubDistrictCommand, string>
{
    private readonly ICpaDatabaseContext _context;

    public CreateSubDistrictCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateSubDistrictCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(request.ProvinceId, request.DistrictId, request.SubDistrictId, request.Name, request.NameEnglish, request.ZipCode);
        entity.ProvinceId = request.ProvinceId;
        entity.DistrictId = request.DistrictId;
        entity.SubDistrictId = request.SubDistrictId;
        entity.Name = request.Name;
        entity.NameEnglish = request.NameEnglish;
        entity.ZipCode = request.ZipCode;
        await _context.SubDistricts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.SubDistrictId;
    }
}
