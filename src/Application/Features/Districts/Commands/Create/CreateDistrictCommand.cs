using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.District;

namespace BigLion.CPA.Application.Features.Districts.Commands.Create;

public class CreateDistrictCommand : IRequest<string>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string ProvinceId { get; set; } = string.Empty;
}

public class CreateDistrictCommandValidator : AbstractValidator<CreateDistrictCommand>
{
    public CreateDistrictCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.NameEnglish).NotEmpty();
        RuleFor(x => x.ProvinceId).NotEmpty();
    }
}

public class CreateDistrictCommandHandler : IRequestHandler<CreateDistrictCommand, string>
{
    private readonly ICpaDatabaseContext _context;

    public CreateDistrictCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateDistrictCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(request.ProvinceId, request.Id, request.Name, request.NameEnglish);
        entity.Id = request.Id;
        entity.Name = request.Name;
        entity.NameEnglish = request.NameEnglish;
        entity.ProvinceId = request.ProvinceId;
        await _context.Districts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
