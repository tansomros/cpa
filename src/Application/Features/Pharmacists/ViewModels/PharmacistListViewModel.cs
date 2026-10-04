using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.Pharmacists.ViewModels;

public class PharmacistListViewModel
{
    public ICollection<PharmacistViewModel> Pharmacists { get; set; }

    public PharmacistListViewModel()
    {
        Pharmacists = new HashSet<PharmacistViewModel>();
    }
}
