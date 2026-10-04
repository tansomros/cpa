using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.Province;

namespace BigLion.CPA.Application.Features.Provinces.Commands.Update;

public class UpdateProvinceCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string? ProvinceGroupId { get; set; }
}

public class UpdateProvinceCommandValidator : AbstractValidator<UpdateProvinceCommand>
{
    public UpdateProvinceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.NameEnglish).NotEmpty();
        RuleFor(x => x.Region).NotEmpty();
    }
}

public class UpdateProvinceCommandHandler : IRequestHandler<UpdateProvinceCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateProvinceCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProvinceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Provinces
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Province", request.Id);

        entity.Name = request.Name;
        entity.NameEnglish = request.NameEnglish;
        entity.Region = request.Region;
        entity.ProvinceGroupId = request.ProvinceGroupId;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
