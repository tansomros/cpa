using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using Entity = BigLion.CPA.Domain.Entities.ProvinceGroup;

namespace BigLion.CPA.Application.Features.ProvinceGroups.Commands.Update;

public class UpdateProvinceGroupCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class UpdateProvinceGroupCommandValidator : AbstractValidator<UpdateProvinceGroupCommand>
{
    public UpdateProvinceGroupCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateProvinceGroupCommandHandler : IRequestHandler<UpdateProvinceGroupCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateProvinceGroupCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProvinceGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProvinceGroups
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("ProvinceGroup", request.Id);

        entity.Name = request.Name;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
