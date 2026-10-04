using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Roles.ViewModels;

public class RoleListViewModel
{
    public ICollection<RoleViewModel> Roles { get; set; }

    public RoleListViewModel()
    {
        Roles = new HashSet<RoleViewModel>();
    }
}
