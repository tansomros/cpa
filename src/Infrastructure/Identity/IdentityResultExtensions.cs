using Microsoft.AspNetCore.Identity;
using BigLion.CPA.Application.Common.Models;

namespace BigLion.CPA.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }
}
