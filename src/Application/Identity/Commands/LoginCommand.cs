using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BigLion.CPA.Application.Features.Users.ViewModel;

namespace BigLion.CPA.Application.Identity.Commands;
public sealed record LoginCommand(
    string Username,
    string Password)
    : IRequest<UserViewModel>;

