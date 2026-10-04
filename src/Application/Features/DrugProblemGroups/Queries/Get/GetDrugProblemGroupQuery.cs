using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.DrugProblemGroups.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.DrugProblemGroup;

namespace BigLion.CPA.Application.Features.DrugProblemGroups.Queries.Get;

public class GetDrugProblemGroupQuery : IRequest<DrugProblemGroupViewModel>
{
    public string Code { get; set; } = string.Empty;
}

public class GetDrugProblemGroupQueryHandler : IRequestHandler<GetDrugProblemGroupQuery, DrugProblemGroupViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDrugProblemGroupQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DrugProblemGroupViewModel> Handle(GetDrugProblemGroupQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.DrugProblemGroups
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("DrugProblemGroup", request.Code);

        return _mapper.Map<DrugProblemGroupViewModel>(entity);
    }
}
