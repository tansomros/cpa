using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.ProvinceGroup;

namespace BigLion.CPA.Application.Features.ProvinceGroups.Commands.Create;

public class CreateProvinceGroupCommand : IRequest<string>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class CreateProvinceGroupCommandValidator : AbstractValidator<CreateProvinceGroupCommand>
{
    public CreateProvinceGroupCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateProvinceGroupCommandHandler : IRequestHandler<CreateProvinceGroupCommand, string>
{
    private readonly ICpaDatabaseContext _context;

    public CreateProvinceGroupCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateProvinceGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity(request.Id, request.Name);
        entity.Id = request.Id;
        entity.Name = request.Name;
        await _context.ProvinceGroups.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
