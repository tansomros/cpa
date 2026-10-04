using System;
using System.Collections.Generic;
using System.Text;

namespace BigLion.CPA.Application.Features.PharmacyGroups.ViewModels;

public class PharmacyGroupListViewModel
{
    public ICollection<PharmacyGroupViewModel> PharmacyGroups { get; set; }

    public PharmacyGroupListViewModel()
    {
        PharmacyGroups = new HashSet<PharmacyGroupViewModel>();
    }
}
