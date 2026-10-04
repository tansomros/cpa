using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Prefixs.ViewModels;

public class PrefixListViewModel
{
    public ICollection<PrefixViewModel> Prefixs { get; set; }

    public PrefixListViewModel()
    {
        Prefixs = new HashSet<PrefixViewModel>();
    }
}
