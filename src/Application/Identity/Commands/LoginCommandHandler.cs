using BigLion.CPA.Application.Features.Users.ViewModel;
using BigLion.CPA.Application.Identity.Interfaces;
namespace BigLion.CPA.Application.Identity.Commands;
public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, UserViewModel>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<UserViewModel> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.LoginAsync(
            request.Username,
            request.Password,
            cancellationToken);
    }
}
