using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.UserRoleAssignments.ViewModels;

public class UserRolesListViewModel
{
    public ICollection<UserRolesViewModel> UserRoles { get; set; }

    public UserRolesListViewModel()
    {
        UserRoles = new HashSet<UserRolesViewModel>();
    }
}
