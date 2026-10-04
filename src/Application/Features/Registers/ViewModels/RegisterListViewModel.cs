using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Registers.ViewModels;

public class RegisterListViewModel
{
    public ICollection<RegisterViewModel> Registers { get; set; }

    public RegisterListViewModel()
    {
        Registers = new HashSet<RegisterViewModel>();
    }
}
