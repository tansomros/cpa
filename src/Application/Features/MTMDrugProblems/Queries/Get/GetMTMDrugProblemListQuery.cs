using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Common.Mappings;
using BigLion.CPA.Application.Common.Models;
using BigLion.CPA.Application.Features.MTMDrugProblems.ViewModels;

namespace BigLion.CPA.Application.Features.MTMDrugProblems.Queries.Get;

public class GetMTMDrugProblemListQuery : IRequest<PaginatedList<MTMDrugProblemViewModel>>
{
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 10;
}

public class GetMTMDrugProblemListQueryValidator : AbstractValidator<GetMTMDrugProblemListQuery>
{
    public GetMTMDrugProblemListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}

public class GetMTMDrugProblemListQueryHandler : IRequestHandler<GetMTMDrugProblemListQuery, PaginatedList<MTMDrugProblemViewModel>>
{
    private readonly ICpaDatabaseContext _context;
    private readonly IMapper _mapper;

    public GetMTMDrugProblemListQueryHandler(ICpaDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<MTMDrugProblemViewModel>> Handle(GetMTMDrugProblemListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MTMDrugProblems.AsNoTracking();

        return await query
            .OrderBy(x => x.UID)
            .ProjectTo<MTMDrugProblemViewModel>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit, cancellationToken);
    }
}
