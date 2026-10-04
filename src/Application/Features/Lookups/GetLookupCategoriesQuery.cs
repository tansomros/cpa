using BigLion.CPA.Application.Common.Security;

namespace BigLion.CPA.Application.Features.Lookups;

[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public record GetLookupCategoriesQuery : IRequest<List<string>>;

public class GetLookupCategoriesQueryHandler : IRequestHandler<GetLookupCategoriesQuery, List<string>>
{
    public Task<List<string>> Handle(GetLookupCategoriesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(LookupRegistry.Categories);
    }
}
