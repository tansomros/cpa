using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Province;

namespace BigLion.CPA.Application.Features.Provinces.Commands.Create;

public class CreateProvinceCommand : IRequest<string>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string? ProvinceGroupId { get; set; }
}

public class CreateProvinceCommandValidator : AbstractValidator<CreateProvinceCommand>
{
    public CreateProvinceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.NameEnglish).NotEmpty();
        RuleFor(x => x.Region).NotEmpty();
    }
}

public class CreateProvinceCommandHandler : IRequestHandler<CreateProvinceCommand, string>
{
    private readonly ICpaDatabaseContext _context;

    public CreateProvinceCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateProvinceCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(request.Region, request.Id, request.Name, request.NameEnglish);
        entity.Id = request.Id;
        entity.Name = request.Name;
        entity.NameEnglish = request.NameEnglish;
        entity.Region = request.Region;
        entity.ProvinceGroupId = request.ProvinceGroupId;
        await _context.Provinces.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
