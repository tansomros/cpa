using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.DrugProblemItems.ViewModels;
using Entity = BigLion.CPA.Domain.Entities.DrugProblemItem;

namespace BigLion.CPA.Application.Features.DrugProblemItems.Queries.Get;

public class GetDrugProblemItemQuery : IRequest<DrugProblemItemViewModel>
{
    public string Code { get; set; } = string.Empty;
}

public class GetDrugProblemItemQueryHandler : IRequestHandler<GetDrugProblemItemQuery, DrugProblemItemViewModel>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetDrugProblemItemQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DrugProblemItemViewModel> Handle(GetDrugProblemItemQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.DrugProblemItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken)
            ?? throw new NotFoundException("DrugProblemItem", request.Code);

        return _mapper.Map<DrugProblemItemViewModel>(entity);
    }
}
