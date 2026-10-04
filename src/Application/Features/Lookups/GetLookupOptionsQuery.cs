using BigLion.CPA.Application.Common.Security;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Lookups;

[Authorize(Policy = CpaPolicies.AllowAnonymous)]
public record GetLookupOptionsQuery : IRequest<List<LookupOptionDto>>
{
    public required string Category { get; init; }
    public string? Language { get; init; }
}

public class GetLookupOptionsQueryHandler : IRequestHandler<GetLookupOptionsQuery, List<LookupOptionDto>>
{
    public Task<List<LookupOptionDto>> Handle(GetLookupOptionsQuery request, CancellationToken cancellationToken)
    {
        var options = LookupRegistry.Get(request.Category, request.Language)
            ?? throw new NotFoundException("LookupCategory", request.Category);

        return Task.FromResult(options);
    }
}
